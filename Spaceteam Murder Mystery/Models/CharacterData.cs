namespace SMM.Models
{
    public record CharacterData(
        string Name,
        string Role,
        string Motto,
        string ImagePath,
        string Description,
        string DeathStory,
        HashSet<Clue> Clues,
        InterviewSet Interviews,
        InterviewSet Accusations
    );
}