namespace SMM.Models;

/// <summary>
/// An object containing all relevant text content for the overarching story,
/// irrespective of any randomization of character guilt, death, or otherwise.
/// </summary>
/// <param name="intro">The introduction to the game.</param>
/// <param name="firstMurder">The content to display before the first murder.</param>
public class Story(string intro, string firstMurder)
{
    public string Intro { get; } = intro;
    public string FirstMurder { get; } = firstMurder;
}