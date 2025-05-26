using System.IO;
using System.Text.RegularExpressions;
using SMM.Models;
using SMM.Models.Helpers;

namespace SMM.Services
{
    public static partial class Parser
    {
        public static string AssetDir { get; } = PathHelper.GetAssetDirectory();
        public static CharacterData ParseCharacter(string characterName)
        {
            string charDir = Path.Combine(AssetDir, "Text", "Characters");

            string[] lineswithEmpty = File.ReadAllLines(Path.Combine(charDir, $"{characterName}.md"));
            string[] lines = [..lineswithEmpty.Where(line => !string.IsNullOrWhiteSpace(line))];

            (string name, string role) = ParseNameAndRole(lines[0]);

            int mottoIndex = Array.FindIndex(lines, line => line.StartsWith("*\""));
            string motto = lines[mottoIndex].Trim('*');

            string profileImagePath = Path.Combine(AssetDir, "Images", "Portraits", $"{characterName}.png");
            string csImagePath = Path.Combine(AssetDir, "Images", "Crime Scenes", $"{characterName}.png");

            string description = lines[mottoIndex + 1];

            int deathIndex = Array.FindIndex(lines, line => line == "## DEATH") + 1;
            string deathStory = lines[deathIndex];

            int cluesIndex = Array.FindIndex(lines, line => line == "## CLUES & HINTS") + 1;
            int interviewsIndex = Array.FindIndex(lines, line => line == "## INTERVIEW RESPONSES") + 1;
            int accusationsIndex = Array.FindIndex(lines, line => line == "## ACCUSATION RESPONSES") + 1;

            string[] clueLines = lines[cluesIndex..(interviewsIndex-1)];
            HashSet<Clue> clues = ParseClues(clueLines);

            string[] interviewLines = lines[interviewsIndex..(accusationsIndex-1)];
            InterviewSet interviews = ParseResponses(interviewLines);

            string[] accusationLines = lines[accusationsIndex..];
            InterviewSet accusations = ParseResponses(accusationLines);

            return new CharacterData(
                name,
                role,
                motto,
                profileImagePath,
                csImagePath,
                description,
                deathStory,
                clues,
                interviews,
                accusations
            );
        }
        public static Story ParseStoryData()
        {
            string[] lineswithEmpty = File.ReadAllLines(Path.Combine(AssetDir, "Text", "Overview.md"));
            string[] lines = [.. lineswithEmpty.Where(line => !string.IsNullOrWhiteSpace(line))];

            int introIndex = Array.FindIndex(lines, line => line == "### Intro") + 1;
            int firstMurderIndex = Array.FindIndex(lines, line => line == "### First Murder Intro") + 1;
            int stopIndex = Array.FindIndex(lines, line => line == "*There's probably going to be more here*");

            string[] introLines = lines[introIndex..(firstMurderIndex - 1)];
            string[] firstMurderLines = lines[firstMurderIndex..stopIndex];

            string intro = string.Join("\n\n", introLines);
            string firstMurder = string.Join("\n\n", firstMurderLines);

            return new Story(intro, firstMurder);
        }

        private static (string, string) ParseNameAndRole(string line)
        {
            string[] parts = line.Replace("# ", "").Split(": ");
            return (parts[0].Trim(), parts[1].Trim());
        }
        private static HashSet<Clue> ParseClues(string[] lines)
        {
            HashSet<Clue> clues = [];
            string characterName = "";
            string imageFolder = "";
            foreach (string line in lines)
            {
                if (line.StartsWith('#'))
                {
                    characterName = line.Trim('#', ' ');
                    imageFolder = Path.Combine(AssetDir, "Images", "Clues", $"{characterName}");
                }
                else if (line.StartsWith('-'))
                {
                    string[] parts = line[2..].Split(": ");
                    string clueName = parts[0].Trim('*', ' ');
                    string clueDesc = parts[1].Trim();

                    string imagePath = Path.Combine(imageFolder, $"{clueName.ToLower().Replace(" ", "-")}.png");

                    clues.Add(new Clue(clueName, clueDesc, characterName, imagePath));
                }
            }
            return clues;
        }
        private static InterviewSet ParseResponses(string[] lines)
        {
            InterviewSet responses = new();

            for (int i = 0; i < lines.Length; i += 3)
            {
                string name = RegexCharacterName().Replace(lines[i], "$1").Trim('#', ' ', ':');
                string innocent = lines[i + 1].Trim('>', ' ');
                string guilty = lines[i + 2].Trim('>', ' ');
                responses.Add(name, new ResponseSet(innocent, guilty));
            }
            return responses;
        }

        [GeneratedRegex(@"\[([A-Za-z]+)\]\(.+\)")]
        private static partial Regex RegexCharacterName();
    }
}