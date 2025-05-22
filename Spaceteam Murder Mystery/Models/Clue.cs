namespace SMM.Models
{
    public class Clue(string name, string description, string characterName)
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string CharacterName { get; } = characterName;
        public bool IsFound { get; set; } = false;
    }
}