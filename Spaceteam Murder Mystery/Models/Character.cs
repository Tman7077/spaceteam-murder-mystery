namespace SMM.Models;

/// <summary>
/// Represents an entire character, including their personal information,
/// relevant image filepaths, quotes, and clues to other characters.
/// Built from a CharacterData object.
/// </summary>
/// <param name="data">All data necessary to create a character. See CharacterData record for details.</param>
public class Character(CharacterData data)
{
    // About the character: static
    public string Name { get; } = data.Name;
    public string ShortName { get; } = data.Name.Split()[0];
    public string Role { get; } = data.Role;
    public string Motto { get; } = data.Motto;
    public string ProfileImagePath { get; } = data.ProfileImagePath;
    public string CrimeSceneImagePath { get; } = data.CrimeSceneImagePath;
    public Direction Facing { get; } = data.Facing;
    public string Description { get; } = data.Description;
    public string DeathStory { get; } = data.DeathStory;
    public HashSet<Clue> Clues { get; } = data.Clues;
    public InterviewSet Interviews { get; } = data.Interviews;
    public InterviewSet Accusations { get; } = data.Accusations;

    // Mutable character status: can change mid-game.
    public bool IsGuilty { get; set; } = false;
    public bool IsAlive { get; set; } = true;

    /// <summary>
    /// Gets the Clue implicating this character in the death of the given victim.
    /// </summary>
    /// <param name="victim">The name of the victim for whom to retrieve a clue.</param>
    /// <returns>The relevant clue.</returns>
    public Clue GetClue(string victim) =>
        Clues.FirstOrDefault(clue => clue.Victim == victim)
            ?? throw new InvalidOperationException($"No clue found for victim '{victim}' in character '{ShortName}'.");
}