namespace SMM.Models;

using System.IO;
using Services;

public class Clue(string clueName, string description, string characterName)
{
    public string Name { get; } = clueName;
    public string Description { get; } = description;
    public string CharacterName { get; } = characterName;
    public bool IsFound { get; set; } = false;

    public string GetImagePath(string owner)
    {
        string assetDir = PathHelper.GetAssetDirectory();
        return Path.Combine(assetDir, "Images", "Crime Scenes", $"{CharacterName}Clues", $"{owner}.png");
    }
}