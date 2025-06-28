namespace SMM.Models;

/// <summary>
/// An object containing all relevant text content for the overarching story,
/// irrespective of any randomization of character guilt, death, or otherwise.
/// </summary>
public class Story
{
    public required string Intro       { get; init; }
    public required string FirstMurder { get; init; }
    public required string Victory     { get; init; }
    public required string Defeat      { get; init; }
}