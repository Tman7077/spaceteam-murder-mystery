namespace SMM.Models.Helpers;

using MessagePack;

/// <summary>
/// A container for all data necessary to save and reload a game.
/// </summary>
[MessagePackObject]
public class GameSave
{
    /// <summary>
    /// Maps the character's short name to their (guilty, alive) states.
    /// </summary>
    [Key(0)]
    public required Dictionary<string, (bool, bool)> CharacterData { get; set; }

    /// <summary>
    /// The name of the difficulty.
    /// </summary>
    [Key(1)]
    public required string Difficulty { get; set; }

    /// <summary>
    /// The short name of the most recent victim.
    /// </summary>
    [Key(2)]
    public required string LastVictim { get; set; }

    /// <summary>
    /// The name of the last view (of a supported type)
    /// that was viewed before the game was saved.
    /// </summary>
    [Key(3)]
    public required string LastViewName { get; set; }

    /// <summary>
    /// <para>
    /// <i>Only present if the last view is a CrimeSceneScreen.</i>
    /// </para>
    /// An array of clue names found at the crime scene.
    /// </summary>
    [Key(4)]
    public string[]? CrimeSceneData { get; set; }

    /// <summary>
    /// <para>
    /// <i>Only present if the last view is a StoryScreen.</i>
    /// </para>
    /// The type of Advance necessary for the StoryScreen,
    /// and possibly the character to which that Advance relates.
    /// </summary>
    [Key(5)]
    public (string, string?)? StoryScreenData { get; set; }
}