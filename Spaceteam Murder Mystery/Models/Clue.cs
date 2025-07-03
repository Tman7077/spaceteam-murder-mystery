namespace SMM.Models;

/// <summary>
/// Contains all information relevant to a single clue, including
/// its textual information, image file path, and "found" state.
/// </summary>
public class Clue
{
    /// <summary>
    /// The "name" of the clue, which is a short description of the item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The more full description of the item. This hints at who the clue is meant to implicate.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The character to whose death this clue is relevant.
    /// </summary>
    public string Victim { get; }

    /// <summary>
    /// The character this clue implicates.
    /// </summary>
    public string Owner { get; }

    /// <summary>
    /// The path to the clue's image file used in the crime scene.
    /// </summary>
    public Uri SceneUri { get; }

    /// <summary>
    /// The path to the clue's image file used in the clue inspection screen.
    /// This one is a more direct, clear look at the item.
    /// </summary>
    public Uri CleanUri { get; }

    /// <summary>
    /// The position at which this clue will be placed in the crime scene.
    /// </summary>
    public int[] CrimeScenePos { get; }

    /// <summary>
    ///  Initializes a new instance of the Clue class with the specified parameters.
    /// </summary>
    /// <param name="clueName">The "name" of the clue, which is a short description of the item.</param>
    /// <param name="description">The more full description of the item. This hints at who the clue is meant to implicate.</param>
    /// <param name="victim">The character to whose death this clue is relevant.</param>
    /// <param name="owner">The character this clue implicates.</param>
    /// <param name="xyz">The position at which this clue will be placed in a crime scene.</param>
    public Clue(string clueName, string description, string victim, string owner, int[] xyz)
    {
        Validator.ValidateShortCharacterName(victim);
        Validator.ValidateShortCharacterName(owner);

        Name          = clueName;
        Description   = description;
        Victim        = victim;
        Owner         = owner;
        CrimeScenePos = xyz;
        string path   = Path.Combine(
            AssetHelper.AssetDirectory,
            "Images",
            "Crime Scenes",
            $"{Victim}Clues",
            $"{Owner}.png");
        string pathClean = path.Replace("Clues", "CluesClean");
        SceneUri         = new Uri(path);
        CleanUri         = new Uri(pathClean);
    }
}