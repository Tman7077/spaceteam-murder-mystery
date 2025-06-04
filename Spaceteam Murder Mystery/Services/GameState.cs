namespace SMM.Services;

using Models;
using Models.Difficulties;
using Models.Helpers;
using System.IO;

/// <summary>
/// A general state tracker for various information pertaining to a game.
/// </summary>
public class GameState
{
    private IDifficulty _difficulty;
    private CharacterSet _characters = new();
    public CharacterSet Characters { get => _characters; }

    /// <summary>
    /// Initializes a GameState given a difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level on which to play the game.</param>
    public GameState(IDifficulty difficulty)
    {
        _difficulty = difficulty;
        LoadCharacters();
    }

    #region Settings
    /// <summary>
    /// Gets the current difficulty level.
    /// </summary>
    /// <returns>The name of the difficulty.</returns>
    public string GetDifficultyName()
    { return _difficulty.GetType().Name[1..]; }

    /// <summary>
    /// Sets the difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level to which to change.</param>
    public void ChangeDifficulty(IDifficulty difficulty)
    { _difficulty = difficulty; }
    #endregion

    #region In-Game Actions
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
        // Select a random character if no name is provided.
        Random r = new();
        if (string.IsNullOrEmpty(name))
        { name = Characters.Keys.ElementAt(r.Next(Characters.Count)); }

        // Select a random character to kill, ignoring guilty characters.
        Character? character;
        do
        {
            if (!Characters.TryGetValue(name, out character))
            { throw new ArgumentException($"Character '{name}' does not exist."); }
            name = Characters.Keys.ElementAt(r.Next(Characters.Count));
        } while (character.IsGuilty);

        // Kill the character and return their short name.
        character.IsAlive = false;
        return character.ShortName;
    }

    /// <summary>
    /// Selects a subset of clues for a certain victim's crime scene
    /// based on the difficulty level and current living characters.
    /// </summary>
    /// <param name="clues">The set of clues to which to add.</param>
    /// <param name="victimName">The victim for whose crime scene to select clues.</param>
    public void SelectClues(ref HashSet<Clue> clues, string victimName)
    {
        if (!Characters.ContainsKey(victimName))
        { throw new ArgumentException($"Character '{victimName}' does not exist."); }

        _difficulty.SelectClues(ref clues, ref _characters, victimName);
    }
    #endregion

    #region Private Methods
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

        _difficulty.SelectGuilty(Characters);
    }
    #endregion
}