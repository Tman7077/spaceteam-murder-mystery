namespace SMM.Services;

using MessagePack;
using System.Configuration;
using System.Threading.Tasks;

/// <summary>
/// Manager for saving and loading data required to cleanly exit/reopen games and save progress.
/// </summary>
public static class GameSaveManager
{
    /// <summary>
    /// Options with which to save the data.
    /// </summary>
    private static readonly MessagePackSerializerOptions Options =
        MessagePackSerializerOptions.Standard.WithCompression(
            MessagePackCompression.Lz4BlockArray);

    /// <summary>
    /// The folder in which to save the file.
    /// </summary>
    private static readonly string _saveFolder =
        Path.GetDirectoryName(
            ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath)!;

    /// <summary>
    /// The name of the file in which to save the game state.
    /// </summary>
    private static readonly string _saveFileName =
        Path.Combine(_saveFolder, "game.state");

    /// <summary>
    /// Saves the game state to a file.
    /// </summary>
    /// <param name="main">The window from which to collect required state information.</param>
    /// <param name="lastView">The last screen shown of a supported type, also for information collection.</param>
    public static async Task SaveGameAsync(MainWindow main, UserControl lastView)
    {
        Directory.CreateDirectory(_saveFolder);

        Dictionary<string, (bool, bool)> characterData = [];
        foreach (KeyValuePair<string, Character> kvp in main.State.Characters)
        { characterData[kvp.Key] = (kvp.Value.IsAlive, kvp.Value.IsGuilty); }

        string difficulty = main.State.Difficulty;

        string lastVictim;
        try
        { lastVictim = main.State.LastVictim; }
        catch (InvalidOperationException)
        { lastVictim = string.Empty; }

        string lastViewName = lastView.GetType().Name;

        GameSave saveData = new()
        {
            CharacterData = characterData,
            Difficulty    = difficulty,
            LastVictim    = lastVictim,
            LastViewName  = lastViewName
        };

        switch (lastView)
        {
            case CrimeSceneScreen css:
                saveData.CrimeSceneData = css.GetSaveData();
                break;
            case StoryScreen ss:
                saveData.StoryScreenData = ss.GetSaveData();
                break;
            default:
                throw new ArgumentException(
                    $"Cannot save state for view type (unsupported screen type): " +
                    $"{lastView.GetType().Name}",
                    nameof(lastView));
        }

        byte[] payload = MessagePackSerializer.Serialize(saveData, Options);
        string tmp     = _saveFileName + ".tmp";
        await File.WriteAllBytesAsync(tmp, payload);
        File.Move(tmp, _saveFileName, overwrite: true);
    }

    /// <summary>
    /// Loads the game state from a file, if it exists, and returns the appropriate screen to display.
    /// </summary>
    /// <param name="main">The window with which to create the UserControl and whose GameState to set.</param>
    /// <returns>The UserControl to load to the screen to resume gameplay.</returns>
    public static async Task<UserControl?> LoadGameAsync(MainWindow main)
    {
        GameSave? save = await ImportGameSaveAsync();
        if (save is null) return null;

        GameState state = new(save.Difficulty);

        if (!string.IsNullOrWhiteSpace(save.LastVictim))
        { state.LastVictim = save.LastVictim; }

        foreach (KeyValuePair<string, (bool, bool)> kvp in save.CharacterData)
        {
            state.Characters[kvp.Key].IsAlive = kvp.Value.Item1;
            state.Characters[kvp.Key].IsGuilty = kvp.Value.Item2;
        }

        main.SetGameState(state);

        UserControl? view = save.LastViewName switch
        {
            nameof(StoryScreen)      => CreateStoryScreen(main, state, save.StoryScreenData),
            nameof(CrimeSceneScreen) => CreateCrimeSceneScreen(main, save.LastVictim, save.CrimeSceneData),
            _ => throw new InvalidOperationException($"Cannot load state for view type (unsupported screen type): {save.LastViewName}")
        };

        return view;
    }

    /// <summary>
    /// Removes the game save file, if it exists (called upon game completion).
    /// </summary>
    public static void RemoveGameSaveAsync()
    {
        if (File.Exists(_saveFileName))
        {
            string tmp = _saveFileName + ".tmp";
            if (File.Exists(tmp)) File.Delete(tmp);
            File.Delete(_saveFileName);
        }
    }

    /// <summary>
    /// Imports the game save from the file, if it exists, and deserializes it into a GameSave object.
    /// </summary>
    /// <returns>A GameSave object containing the deserialized data.</returns>
    private static async Task<GameSave?> ImportGameSaveAsync()
    {
        if (!File.Exists(_saveFileName)) return null;

        byte[] payload = await File.ReadAllBytesAsync(_saveFileName);
        return MessagePackSerializer.Deserialize<GameSave>(payload, Options);
    }

    /// <summary>
    /// Creates a StoryScreen based on the provided save data.
    /// </summary>
    /// <param name="main">The window with which to create the UserControl.</param>
    /// <param name="state">The GameState with which to create a new Vote, if necessary.</param>
    /// <param name="saveData">The required information with which to create the StoryScreen.</param>
    /// <returns>A complete StoryScreen UserControl.</returns>
    private static StoryScreen CreateStoryScreen(MainWindow main, GameState state, (string, string?)? saveData)
    {
        (string advanceType, string? name) = saveData
            ?? throw new InvalidOperationException("Story screen data is missing in save file.");

        StoryScreen ss;
        switch (advanceType)
        {
            case nameof(Advance.Intro):
                ss = new StoryScreen(main, new Advance.Intro());
                break;
            case nameof(Advance.FirstMurder):
                ss = new StoryScreen(main, new Advance.FirstMurder());
                break;
            case nameof(Advance.PreDeath):
                if (name is null)
                { throw new InvalidOperationException("Victim is required for PreDeath advance."); }
                ss = new StoryScreen(main, new Advance.PreDeath(name));
                break;
            case nameof(Advance.PostDeath):
                ss = new StoryScreen(main, new Advance.PostDeath());
                break;
            case nameof(Advance.PostAccusation):
                if (name is null)
                { throw new InvalidOperationException("Name is required for PostAccusation advance."); }
                Vote vote;
                if (name == string.Empty)
                { vote = new Vote.None(); }
                else vote = new(name, state);
                ss = new StoryScreen(main, new Advance.PostAccusation(vote));
                break;
            default:
                throw new InvalidOperationException($"Unknown advance type: {advanceType}");
        }

        return ss;
    }

    /// <summary>
    /// Creates a CrimeSceneScreen based on the provided save data.
    /// </summary>
    /// <param name="main">The window with which to create the UserControl.</param>
    /// <param name="lastVictim">The victim whose scene to load.</param>
    /// <param name="saveData">The list of clues to load.</param>
    /// <returns>A completed CrimeSceneScreen UserControl.</returns>
    private static CrimeSceneScreen CreateCrimeSceneScreen(MainWindow main, string lastVictim, string[]? saveData)
    {
        string[] clueOwners = saveData
            ?? throw new InvalidOperationException("Crime scene data is missing in save file.");

        List<Clue> clues = [];
        foreach (string owner in clueOwners)
        {
            if (main.State.Characters[owner].GetClue(lastVictim) is not Clue clue)
            { throw new InvalidOperationException($"Clue with owner '{owner}' not found in game state."); }
            else clues.Add(clue);
        }

        return new CrimeSceneScreen(main, lastVictim, clues);
    }
}