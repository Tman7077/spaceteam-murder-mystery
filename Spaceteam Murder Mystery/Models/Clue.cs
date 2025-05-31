namespace SMM.Models;

using Services;
using System.IO;

public class Clue
{
    public string Name { get; }
    public string Description { get; }
    public string Victim { get; }
    public string Owner { get; }
    public string ImagePath { get; }
    public bool IsFound { get; set; }

    public Clue(string clueName, string description, string victim, string owner)
    {
        Name = clueName;
        Description = description;
        Victim = victim;
        Owner = owner;
        ImagePath = Path.Combine(PathHelper.GetAssetDirectory(), "Images", "Crime Scenes", $"{Victim}Clues", $"{Owner}.png");
        IsFound = false;
    }
}