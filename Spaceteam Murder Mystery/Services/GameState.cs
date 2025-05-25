using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using System.IO;

namespace SMM.Services
{
    public class GameState
    {
        public Dictionary<string, Character> Characters { get; } = null!;
        public IDifficulty Difficulty { get; set; } = null!;

        public GameState(IDifficulty difficulty)
        {
            StartReset(difficulty);
        }
        public void StartReset(IDifficulty? difficulty = null)
        {
            Difficulty = difficulty ?? Difficulty;
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            Characters.Clear();
            string assetDir = PathHelper.GetAssetDirectory();

            foreach (string characterFile in Directory.GetFiles(assetDir, "*.md"))
            {
                string characterName = Path.GetFileNameWithoutExtension(characterFile);
                CharacterData data = Parser.ParseCharacter(characterName);
                Characters[characterName] = new Character(data);
            }
            Difficulty.SelectGuilty(Characters);
        }
    }
}