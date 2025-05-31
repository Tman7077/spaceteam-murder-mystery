namespace SMM.Services;

using Models;
using Models.Difficulties;
using Models.Helpers;
using System.IO;

public class GameState
{
    private IDifficulty _difficulty;
    private CharacterSet _characters = new();
    public CharacterSet Characters { get => _characters; }

    public GameState(IDifficulty difficulty)
    {
        _difficulty = difficulty;
        LoadCharacters();
    }

    #region Settings
    public string GetDifficulty()
    {
        return _difficulty.GetType().Name[1..];
    }
    public void ChangeDifficulty(IDifficulty difficulty)
    {
        _difficulty = difficulty;
    }
    #endregion

    #region In-Game Actions
    public string KillCharacter(string? name)
    {
        if (string.IsNullOrEmpty(name))
        { name = Characters.Keys.ElementAt(new Random().Next(Characters.Count)); }

        if (!Characters.TryGetValue(name, out Character? character))
        { throw new ArgumentException($"Character '{name}' does not exist."); }

        character.IsAlive = false;
        return character.Name;
    }
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