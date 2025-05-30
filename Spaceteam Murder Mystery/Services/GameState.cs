namespace SMM.Services;

using Models;
using Models.Difficulties;
using Models.Helpers;
using System.IO;

public class GameState
{
    public Dictionary<string, Character> Characters { get; } = [];
    public IDifficulty Difficulty { get; set; } = null!;

    public GameState(IDifficulty difficulty)
    {
        StartReset(difficulty);
    }
    public void StartReset(IDifficulty? difficulty = null)
    {
        ChangeDifficulty(difficulty);
        LoadCharacters();
    }
    public void ChangeDifficulty(IDifficulty? difficulty = null)
    {
        Difficulty = difficulty ?? Difficulty;
    }
    public string GetDifficulty()
    {
        return Difficulty.GetType().Name[1..];
    }

    private void LoadCharacters()
    {
        Characters.Clear();
        string assetDir = PathHelper.GetAssetDirectory();
        string charDir = Path.Combine(assetDir, "Text", "Characters");

        foreach (string characterFile in Directory.GetFiles(charDir, "*.md"))
        {
            string characterName = Path.GetFileNameWithoutExtension(characterFile);
            CharacterData data = Parser.ParseCharacter(characterName);
            Characters[characterName] = new Character(data);
        }
        Difficulty.SelectGuilty(Characters);
    }
}