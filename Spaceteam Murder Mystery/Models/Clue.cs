namespace SMM.Models;

using System.IO;

/// <summary>
/// Contains all information relevant to a single clue, including
/// its textual information, image file path, and "found" state.
/// </summary>
public class Clue
{
    public string Name        { get; }
    public string Description { get; }
    public string Victim      { get; }
    public string Owner       { get; }
    public Uri SceneUri       { get; }
    public Uri CleanUri       { get; }
    public int X   { get; }
    public int Y   { get; }
    public int Z   { get; }
    public bool IsFound { get; set; }

    /// <summary>
    ///  Initializes a new instance of the Clue class with the specified parameters.
    /// </summary>
    /// <param name="clueName">The "name" of the clue, which is a short description of the item.</param>
    /// <param name="description">The more full description of the item. This hints at who the clue is meant to implicate.</param>
    /// <param name="victim">The character to whose death this clue is relevant.</param>
    /// <param name="owner">The character this clue implicates.</param>
    public Clue(string clueName, string description, string victim, string owner, int[] xyz)
    {
        Validator.ValidateShortCharacterName(victim);
        Validator.ValidateShortCharacterName(owner);

        Name        = clueName;
        Description = description;
        Victim      = victim;
        Owner       = owner;
        X = xyz[0];
        Y = xyz[1];
        Z = xyz[2];
        string path = Path.Combine(
            PathHelper.GetAssetDirectory(),
            "Images",
            "Crime Scenes",
            $"{Victim}Clues",
            $"{Owner}.png");
        string pathClean = path.Replace("Clues", "CluesClean");
        SceneUri = new Uri(path);
        CleanUri = new Uri(pathClean);
        IsFound = false;
    }
}