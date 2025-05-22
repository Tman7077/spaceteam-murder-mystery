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
        public HashSet<Clue> Clues { get; } = data.Clues;
        public InterviewSet Interviews { get; } = data.Interviews;
        public InterviewSet Accusations { get; } = data.Accusations;

        // Mutable character status: change mid-game and game-to-game
        public bool IsSuspect { get; set; } = isSuspect;
        public bool IsAlive { get; set; } = true;
    }
}