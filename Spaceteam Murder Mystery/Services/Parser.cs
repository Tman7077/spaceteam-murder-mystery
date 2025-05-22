using System.IO;
using System.Text.RegularExpressions;
using SMM.Models;

namespace SMM.Services
{
    public static partial class Parser
    {
        public static string ProjectRoot { get; } = PathHelper.GetProjectRoot();
        public static CharacterData ParseCharacter(string characterName)
        {
            string assetDir = Path.Combine(ProjectRoot, "Assets");
            string charDir = Path.Combine(assetDir, "Text", "Characters");

            string[] lineswithEmpty = File.ReadAllLines(Path.Combine(charDir, $"{characterName}.md"));
            string[] lines = [..lineswithEmpty.Where(line => !string.IsNullOrWhiteSpace(line))];

            (string name, string role) = ParseNameAndRole(lines[0]);

            int mottoIndex = Array.FindIndex(lines, line => line.StartsWith("*\""));
            string motto = lines[mottoIndex].Trim('*');

            string imagePath = Path.Combine(assetDir, "Images", "Portraits", $"{characterName}.png");

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
                imagePath,
                description,
                deathStory,
                clues,
                interviews,
                accusations
            );
        }
        public static (string, string) ParseNameAndRole(string line)
        {
            string[] parts = line.Replace("# ", "").Split(": ");
            if (parts.Length != 2)
            {
                throw new FormatException($"Invalid name and role format: {line}");
            }
            return (parts[0].Trim(), parts[1].Trim());
        }
        public static HashSet<Clue> ParseClues(string[] lines)
        {
            HashSet<Clue> clues = [];
            string characterName = "";
            foreach (string line in lines)
            {
                if (line.StartsWith('#'))
                {
                    characterName = line.Trim('#', ' ');
                }
                else // if (line.StartsWith('-'))
                {
                    string[] parts = line[2..].Split(": ");
                    if (parts.Length == 2)
                    {
                        clues.Add(new Clue(parts[0].Trim('*', ' '), parts[1].Trim(), characterName));
                    }
                }
            }
            return clues;
        }
        public static InterviewSet ParseResponses(string[] lines)
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