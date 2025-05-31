namespace SMM.Models;

using Helpers;

public class Character(CharacterData data)
{
    // About the character: static
    public string Name { get; } = data.Name;
    public string Role { get; } = data.Role;
    public string Motto { get; } = data.Motto;
    public string ProfileImagePath { get; } = data.ProfileImagePath;
    public string CrimeSceneImagePath { get; } = data.CrimeSceneImagePath;
    public string Description { get; } = data.Description;
    public string DeathStory { get; } = data.DeathStory;
    public HashSet<Clue> Clues { get; } = data.Clues;
    public InterviewSet Interviews { get; } = data.Interviews;
    public InterviewSet Accusations { get; } = data.Accusations;

    // Mutable character status: can change mid-game.
    public bool IsGuilty { get; set; } = false;
    public bool IsAlive { get; set; } = true;

    public Clue GetClue(string victim)
    {
        return Clues.FirstOrDefault(clue => clue.Victim == victim) ?? throw new InvalidOperationException($"No clue found for victim '{victim}' in character '{Name}'.");
    }
}