namespace SMM.Models;

/// <summary>
/// An object containing all relevant text content for the overarching story,
/// irrespective of any randomization of character guilt, death, or otherwise.
/// </summary>
public class Story
{
    /// <summary>
    /// Text to display to introduce the story.
    /// </summary>
    public required string Intro { get; init; }

    /// <summary>
    /// Text to display before the first murder occurs.
    /// </summary>
    public required string FirstMurder { get; init; }
    
    /// <summary>
    /// Text to display when the player completes a game
    /// and wins.
    /// </summary>
    public required string Victory { get; init; }
    
    /// <summary>
    /// Text to display when the player completes a game
    /// and loses.
    /// </summary>
    public required string Defeat { get; init; }
}