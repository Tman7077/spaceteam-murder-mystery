namespace SMM.Services;

using System.IO;

/// <summary>
/// A general state tracker for various information pertaining to a game.
/// </summary>
public class GameState
{
    private readonly CharacterSet _characters = [];
    
    public CharacterSet Characters { get => _characters; }
    public string Difficulty { get; set; }

    /// <summary>
    /// Initializes a GameState given a difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level on which to play the game.</param>
    public GameState(string difficulty)
    {
        Difficulty = difficulty;
        LoadCharacters();
    }

    /// <summary>
    /// Kills a character. If a name is not provided, kill a random character.
    /// </summary>
    /// <param name="name">The name of a character to kill.</param>
    /// <returns>
    /// The short name of the character killed.
    /// This will be equal to the <b>name</b> argument, if it is provided.
    /// </returns>
    public string KillCharacter(string? name = null)
    {
        // Select a random character if no name is provided. Ignores guilty characters.
        List<string> livingInnocents = Characters.GetLivingNames(includeGuilty: false);

        // Make sure there are living characters available to kill.
        if (livingInnocents.Count == 0)
        { throw new InvalidOperationException("No living characters available to kill."); }

        if (string.IsNullOrEmpty(name))
        { name = livingInnocents[new Random().Next(livingInnocents.Count)]; }

        // Kill the character and return their short name.
        Character character =  Characters[name];
        character.IsAlive = false;
        return character.ShortName;
    }

    /// <summary>
    /// Selects a subset of clues for a certain victim's crime scene
    /// based on the difficulty level and current living characters.
    /// </summary>
    /// <param name="clues">The set of clues to which to add.</param>
    /// <param name="victimName">The victim for whose crime scene to select clues.</param>
    public void SelectClues(HashSet<Clue> clues, string victimName)
    {
        if (!Characters.ContainsKey(victimName))
        { throw new ArgumentException($"Character '{victimName}' does not exist."); }

        Difficulties.All[Difficulty].SelectClues(clues, _characters, victimName);
    }
    
    private void LoadCharacters()
    {
        Characters.Clear();
        string assetDir = PathHelper.GetAssetDirectory();
        string charDir = Path.Combine(assetDir, "Text", "Characters");

        foreach (string charFile in Directory.GetFiles(charDir, "*.md"))
        {
            string charName = Path.GetFileNameWithoutExtension(charFile);
            CharacterData data = Parser.ParseCharacter(charName);
            Characters[charName] = new Character(data);
        }

        Difficulties.All[Difficulty].SelectGuilty(Characters);
    }
}