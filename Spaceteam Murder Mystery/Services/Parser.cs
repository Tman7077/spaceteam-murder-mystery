namespace SMM.Services;

using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// A helper class used to read in data from the character and story markdown files.
/// </summary>
public static partial class Parser
{
    private static readonly string _assetDir = PathHelper.GetAssetDirectory();

    /// <summary>
    /// Parses a character's data from the markdown file with the given name.
    /// </summary>
    /// <param name="characterName">The short name of a character, aligning with a filename to parse.</param>
    /// <returns>The data necessary to create a Character.</returns>
    public static Character ParseCharacter(string characterName)
    {
        string charDir = Path.Combine(_assetDir, "Text", "Characters");

        string[] lineswithEmpty = File.ReadAllLines(Path.Combine(charDir, $"{characterName}.md"));
        string[] lines = [.. lineswithEmpty.Where(line => !string.IsNullOrWhiteSpace(line))];

        (string name, string role) = ParseNameAndRole(lines[0]);

        int mottoIndex = Array.FindIndex(lines, line => line.StartsWith("*\""));
        string motto   = lines[mottoIndex].Trim('*');

        string profileImagePath    = Path.Combine(_assetDir, "Images", "Portraits", $"{characterName}.png");
        string endProfileImagePath = Path.Combine(_assetDir, "Images", "Portraits", "EndScreen", $"{characterName}.png");
        string csImagePath         = Path.Combine(_assetDir, "Images", "Crime Scenes", $"{characterName}.png");

        Uri profileUri    = new(profileImagePath);
        Uri endProfileUri = new(endProfileImagePath);
        Uri csUri         = new(csImagePath);

        int DirLocIndex = Array.FindIndex(lines, line => line.StartsWith('`'));
        int split       = lines[DirLocIndex].IndexOf("` `");

        string dirUnparsed  = lines[DirLocIndex][..split];
        Direction direction = dirUnparsed.Split(':')[1].Trim('`', ' ') == "Left" ? Direction.Left : Direction.Right;

        string locationUnparsed = lines[DirLocIndex][split..];
        int[]  xyz = [..locationUnparsed.Trim('`', ' ').Split(',').Select(int.Parse)];

        string description = lines[DirLocIndex + 1];

        int    preDeathIndex  = Array.FindIndex(lines, line => line == "### Pre-Death Blurb") + 1;
        string preDeathBlurb  = lines[preDeathIndex];
        int    postDeathIndex = Array.FindIndex(lines, line => line == "### Post-Death Blurb") + 1;
        string postDeathBlurb = lines[postDeathIndex];

        int cluesIndex       = Array.FindIndex(lines, line => line == "## CLUES & HINTS") + 1;
        int interviewsIndex  = Array.FindIndex(lines, line => line == "## INTERVIEW RESPONSES") + 1;
        int accusationsIndex = Array.FindIndex(lines, line => line == "## ACCUSATION RESPONSES") + 1;

        string[] clueLines  = lines[cluesIndex..(interviewsIndex - 1)];
        HashSet<Clue> clues = ParseClues(clueLines, characterName);

        string[] interviewLines = lines[interviewsIndex..(accusationsIndex - 1)];
        InterviewSet interviews = ParseResponses(interviewLines);

        string[] accusationLines = lines[accusationsIndex..];
        InterviewSet accusations = ParseResponses(accusationLines);

        return new Character()
        {
            Name           = name,
            ShortName      = name.Split()[0],
            Role           = role,
            Motto          = motto,
            ProfileUri     = profileUri,
            EndProfileUri  = endProfileUri,
            CrimeSceneUri  = csUri,
            Facing         = direction,
            EndScreenPos   = xyz,
            Description    = description,
            PreDeathBlurb  = preDeathBlurb,
            PostDeathBlurb = postDeathBlurb,
            Clues          = clues,
            Interviews     = interviews,
            Accusations    = accusations
        };
    }

    /// <summary>
    /// Parses the story data from the overview markdown file.
    /// </summary>
    /// <returns>A complete Story object with all relevant information.</returns>
    public static Story ParseStoryData()
    {
        string[] lineswithEmpty = File.ReadAllLines(Path.Combine(_assetDir, "Text", "Overview.md"));
        string[] lines = [.. lineswithEmpty.Where(line => !string.IsNullOrWhiteSpace(line))];

        int introIndex       = Array.FindIndex(lines, line => line == "### Intro") + 1;
        int firstMurderIndex = Array.FindIndex(lines, line => line == "### First Murder Intro") + 1;
        int victoryIndex     = Array.FindIndex(lines, line => line == "### Victory") + 1;
        int defeatIndex      = Array.FindIndex(lines, line => line == "### Defeat") + 1;

        string[] introLines = lines[introIndex..(firstMurderIndex - 1)];

        string intro       = string.Join("\n\n", introLines);
        string firstMurder = lines[firstMurderIndex];
        string victory     = lines[victoryIndex];
        string defeat      = lines[defeatIndex];

        return new Story
        {
            Intro       = intro,
            FirstMurder = firstMurder,
            Victory     = victory,
            Defeat      = defeat
        };
    }

    private static (string, string) ParseNameAndRole(string line)
    {
        string[] parts = line.Replace("# ", "").Split(": ");
        return (parts[0].Trim(), parts[1].Trim());
    }

    private static HashSet<Clue> ParseClues(string[] lines, string owner)
    {
        HashSet<Clue> clues = [];
        string deadCharName = "";
        foreach (string line in lines)
        {
            if (line.StartsWith('#'))
            { deadCharName = line.Trim('#', ' '); }
            else if (line.StartsWith('-'))
            {
                string[] parts         = line[2..].Split(": ");
                string[] nameAndCoords = parts[0].Split('(');
                string   clueName      = nameAndCoords[0].Trim('*', ' ');
                string[] coords        = nameAndCoords[1].Trim(')').Split(',');
                int[]    xyz           = [.. coords.Select(int.Parse)];
                string   clueDesc      = parts[1].Trim();

                clues.Add(new Clue(clueName, clueDesc, deadCharName, owner, xyz));
            }
        }
        return clues;
    }

    private static InterviewSet ParseResponses(string[] lines)
    {
        InterviewSet responses = new();

        for (int i = 0; i < lines.Length; i += 3)
        {
            string name     = RegexCharacterName().Replace(lines[i], "$1").Trim('#', ':', ' ');
            string innocent = lines[i + 1].Trim('>', ' ');
            string guilty   = lines[i + 2].Trim('>', ' ');
            responses.Add(name, new ResponseSet(innocent, guilty));
        }
        return responses;
    }

    [GeneratedRegex(@"\[([A-Za-z]+)\]\(.+\)")]
    private static partial Regex RegexCharacterName();
}