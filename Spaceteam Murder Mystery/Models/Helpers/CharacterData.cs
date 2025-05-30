namespace SMM.Models.Helpers;

public record CharacterData(
    string Name,
    string Role,
    string Motto,
    string ProfileImagePath,
    string CrimeSceneImagePath,
    string Description,
    string DeathStory,
    HashSet<Clue> Clues,
    InterviewSet Interviews,
    InterviewSet Accusations
);