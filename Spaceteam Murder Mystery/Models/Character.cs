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

    // Mutable character status: change mid-game and game-to-game
    public bool IsSuspect { get; set; } = false;
    public bool IsAlive { get; set; } = true;
}