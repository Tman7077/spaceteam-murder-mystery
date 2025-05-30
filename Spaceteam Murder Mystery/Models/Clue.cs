namespace SMM.Models;

using Services;
using System.IO;

public class Clue
{
    public string Name { get; }
    public string Description { get; }
    public string Victim { get; }
    public string Owner { get; set; }
    public string ImagePath { get; set; }
    public bool IsFound { get; set; }

    public Clue(string clueName, string description, string victim, string owner)
    {
        Name = clueName;
        Description = description;
        Victim = victim;
        Owner = owner;
        ImagePath = GetImagePath();
        IsFound = false;
    }
    private string GetImagePath()
    {
        string assetDir = PathHelper.GetAssetDirectory();
        return Path.Combine(assetDir, "Images", "Crime Scenes", $"{Victim}Clues", $"{Owner}.png");
    }
}