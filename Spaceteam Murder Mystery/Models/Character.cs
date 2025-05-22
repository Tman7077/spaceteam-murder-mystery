using System.Xml;

namespace SMM.Models
{
    public class Character(CharacterData data, bool isSuspect)
    {
        // About the character: static
        public string Name { get; } = data.Name;
        public string Role { get; } = data.Role;
        public string Motto { get; } = data.Motto;
        public string ImagePath { get; } = data.ImagePath;
        public string Description { get; } = data.Description;
        public string DeathStory { get; } = data.DeathStory;

        public List<Clue> Clues { get; } = data.Clues;
        public InterviewSet Interviews { get; } = data.Interviews;
        public InterviewSet Accusations { get; } = data.Accusations;

        // Mutable character status: change mid-game and game-to-game
        public bool IsSuspect { get; set; } = isSuspect;
        public bool IsAlive { get; set; } = true;
    }
    // public class Character(string name, string role, string motto, string description, string deathStory, string imagePath, List<Clue> clues, Dictionary<string, string> interviews, Dictionary<string, string> accusations, bool isSuspect)
    // {
    //     // About the character: static
    //     public string Name { get; } = name;
    //     public string Role { get; } = role;
    //     public string Motto { get; } = motto;
    //     public string Description { get; } = description;
    //     public string DeathStory { get; } = deathStory;
    //     public string ImagePath { get; } = imagePath;

    //     public List<Clue> Clues { get; } = clues;
    //     public Dictionary<string, string> Interviews { get; } = interviews;
    //     public Dictionary<string, string> Accusations { get; } = accusations;

    //     // Mutable character status: change mid-game and game-to-game
    //     public bool IsSuspect { get; set; } = isSuspect;
    //     public bool IsAlive { get; set; } = true;
    // }
}