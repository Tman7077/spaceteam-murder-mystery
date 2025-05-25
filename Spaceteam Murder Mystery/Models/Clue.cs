namespace SMM.Models
{
    public class Clue(string name, string description, string characterName, string imagePath)
    {
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string CharacterName { get; } = characterName;
        public string ImagePath { get; } = imagePath;
        public bool IsFound { get; set; } = false;
    }
}