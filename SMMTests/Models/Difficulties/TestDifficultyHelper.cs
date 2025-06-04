namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using System.Buffers;
using System.Diagnostics;
using Xunit.Abstractions;

public static class TestDifficultyHelper
{
    private static readonly string[] _fullCharacterNames = [
        "Alex Blackhole", "Cade Morningstar", "Colter Helix", "Courtney Lunaris",
        "Ethan Aurora", "Jacie Starwalker", "Olsen Horizon", "Tyler Bytewell"
    ];
    private static readonly string[] _shortCharacterNames = [.. _fullCharacterNames.Select(name => name.Split()[0])];
    public static TheoryData<string> ShortCharacterNames { get; } = [.. _shortCharacterNames];
    public static CharacterSet MockCharSet()
    {
        CharacterSet testChars = new();

        string role = "testRoleUnused";
        string motto = "testMottoUnused";
        string profileImagePath = "testProfileImagePathUnused";
        string crimeSceneImagePath = "testCrimeSceneImagePathUnused";
        string description = "testDescriptionUnused";
        string deathStory = "testDeathStoryUnused";
        InterviewSet interviews = new();
        InterviewSet accusations = new();

        foreach (string fullName in _fullCharacterNames)
        {
            string shortName = fullName.Split()[0];
            HashSet<Clue> testClueSet = [];
            foreach (string victim in _shortCharacterNames)
            {
                Clue testClue = new("testName", "testDescription", victim, shortName);
                testClueSet.Add(testClue);
            }
            CharacterData cData = new(
                fullName, role, motto, profileImagePath, crimeSceneImagePath,
                description, deathStory, testClueSet, interviews, accusations
            );
            Character testChar = new(cData);
            testChars[shortName] = testChar;
        }

        return testChars;
    }

    public static void TestSelectClues(string[] guilty, IDifficulty diff, Action<HashSet<Clue>, CharacterSet, string[]> difficultyAsserts, ITestOutputHelper output)
    {
        CharacterSet characters = MockCharSet();
        HashSet<Clue> clues = [];
        IDifficulty difficulty = diff;
        foreach (string owner in guilty)
        { characters[owner].IsGuilty = true; }

        // I know I manually set this, but just for checking :)
        if (Debugger.IsAttached)
        { output.WriteLine(new string('-', 50) + $"Guilty Character selected: {characters.Values.FirstOrDefault(c => c.IsGuilty)?.ShortName ?? "None"}"); }

        string[] innocent = [.. _shortCharacterNames.Where(name => !guilty.Contains(name))];
        foreach (string victim in innocent)
        {
            characters[victim].IsAlive = false;
            List<string> livingInnocent = characters.GetLivingNames(omitGuilty: true);

            for (int i = 0; i < livingInnocent.Count; i++)
            {
                characters[livingInnocent[i]].IsAlive = false;

                for (int j = 1; j <= 50; j++)
                {
                    if (Debugger.IsAttached)
                    { output.WriteLine(new string('-', 50) + $"\nIteration {j} for victim: {victim}\n" + new string('-', 50)); }

                    try
                    {
                        difficulty.SelectClues(ref clues, ref characters, victim);

                        if (Debugger.IsAttached)
                        {
                            output.WriteLine($"One of the below clues should contain an owner in the following: {string.Join(", ", guilty)}");
                            foreach (Clue c in clues)
                            {
                                output.WriteLine(new string('v', 25));
                                output.WriteLine($"    Owner : {c.Owner}");
                                output.WriteLine($"   Victim : {c.Victim}");
                                output.WriteLine($"ImagePath : {c.ImagePath}");
                                output.WriteLine(new string('^', 25));
                            }
                        }
                        
                        // -------------------------
                        // This is  where the actual assertions are made
                        // -------------------------
                        difficultyAsserts(clues, characters, guilty);

                        clues.Clear();
                    }
                    catch (Exception e)
                    { Assert.Fail($"Iteration {j} with {livingInnocent.Count - i - 1} innocent living characters threw an exception: {e.Message}"); }
                }
            }

            foreach (Character character in characters.Values)
            { character.IsAlive = true; }
        }
    }
}