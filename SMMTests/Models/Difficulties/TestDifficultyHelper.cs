namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using System.Diagnostics;
using Xunit.Abstractions;


public static class TestDifficultyHelper
{
    // Hard-coded names of the characters, full and short names
    private static readonly string[] _fullCharacterNames = [
        "Alex Blackhole", "Cade Morningstar", "Colter Helix", "Courtney Lunaris",
        "Ethan Aurora", "Jacie Starwalker", "Olsen Horizon", "Tyler Bytewell"
    ];
    private static readonly string[] _shortCharacterNames = [.. _fullCharacterNames.Select(name => name.Split()[0])];
    public delegate void DifficultyAssertions(HashSet<Clue> clues, CharacterSet characters);

    // Expose the short character names as TheoryData for use in tests
    public static TheoryData<string> ShortCharacterNames { get; } = [.. _shortCharacterNames];

    /// <summary>
    /// Creates a mock CharacterSet with test data for all characters,
    /// for use in testing the SelectClues method in various difficulties.
    /// The characters are initialized with a set of clues that point to all other characters as victims.
    /// </summary>
    /// <returns> A complete CharacterSet containing test characters with clues pointing to each other. </returns>
    public static CharacterSet MockCharSet()
    {
        CharacterSet testChars = [];

        // None of these values are actually used in the tests,
        // but they are required to create a Character.
        string       role           = "testRoleUnused";
        string       motto          = "testMottoUnused";
        Uri          profileUri     = new("testProfileUriUnused",    UriKind.RelativeOrAbsolute);
        Uri          crimeSceneUri  = new("testCrimeSceneUriUnused", UriKind.RelativeOrAbsolute);
        Direction    facing         = Direction.Left;
        string       description    = "testDescriptionUnused";
        string       preDeathBlurb  = "testPreDeathBlurbUnused";
        string       postDeathBlurb = "testPostDeathBlurbUnused";
        int[]        xyz            = [0, 0, 0];
        InterviewSet interviews     = new();
        InterviewSet accusations    = new();

        // Using full names here, because the Character class expects full names.
        foreach (string fullName in _fullCharacterNames)
        {
            string shortName = fullName.Split()[0];
            HashSet<Clue> testClueSet = [];

            // Create clues for each character, where each clue points to every other character as a victim.
            foreach (string victim in _shortCharacterNames)
            {
                Clue testClue = new("testName", "testDescription", victim, shortName, xyz);
                testClueSet.Add(testClue);
            }
            Character testChar = new()
            {
                Name           = fullName,
                ShortName      = shortName,
                Role           = role,
                Motto          = motto,
                ProfileUri     = profileUri,
                CrimeSceneUri  = crimeSceneUri,
                Facing         = facing,
                Description    = description,
                PreDeathBlurb  = preDeathBlurb,
                PostDeathBlurb = postDeathBlurb,
                Clues          = testClueSet,
                Interviews     = interviews,
                Accusations    = accusations
            };
            testChars[shortName] = testChar;
        }

        return testChars;
    }

    /// <summary>
    /// Tests the SelectClues method of a given difficulty by simulating various scenarios.
    /// It iterates through all combinations of guilty characters and victims,
    /// ensuring that the clues selected are valid according to the difficulty's rules.
    /// Most of the assertions are made in the DifficultyAssertions parameter <b>asserts</b>.
    /// </summary>
    /// <param name="guilty">One or more characters to mark as guilty</param>
    /// <param name="diff">A difficulty class based on the IDifficulty interface</param>
    /// <param name="asserts">A method containing the actual assertions that depend on the difficulty</param>
    /// <param name="output">A test output to which to write for debugging</param>
    public static void TestSelectClues(string[] guilty, string diff, DifficultyAssertions asserts, ITestOutputHelper output)
    {
        // This method is intended to be used in unit tests to ensure that the SelectClues
        // method behaves correctly across different scenarios and difficulties.

        // Now look—I know there are a lot of nested loops.
        // I understand that it is a ton of iterations. That is the point.
        // The goal is to ensure that every possible scenario is tested,
        // and when randomness is onvolved, that requires some extra "just in case" iterations.

        CharacterSet characters = MockCharSet();
        HashSet<Clue> clues = [];

        // Set the guilty characters' IsGuilty property in the CharacterSet
        foreach (string owner in guilty)
        { characters[owner].IsGuilty = true; }

        // I know I manually set this, but just for checking :)
        if (Debugger.IsAttached)
        { output.WriteLine(new string('-', 30) + $"Guilty Character selected: {characters.Values.FirstOrDefault(c => c.IsGuilty)?.ShortName ?? "None"}"); }

        // By filtering out guilty, it is ensured that the guilty characters are not selected as victims.
        string[] innocent = [.. _shortCharacterNames.Where(name => !guilty.Contains(name))];
        foreach (string victim in innocent)
        {
            // Set the victim to unalive, and greab everyone else.
            characters[victim].IsAlive = false;
            List<string> livingInnocent = characters.GetLivingNames(includeGuilty: false);

            // This loops through all living characters,
            // selects clues for the remaining group,
            // losing one character per iteration,
            // until there are no more living characters.
            for (int i = 0; i < livingInnocent.Count; i++)
            {
                characters[livingInnocent[i]].IsAlive = false;

                for (int j = 1; j <= 50; j++)
                {
                    if (Debugger.IsAttached)
                    {
                        output.WriteLine(
                            new string('-', 50) +
                            $"\nIteration {j} for victim: {victim}\n" +
                            $"Difficulty: {diff} | " +
                            $"Living Characters: {livingInnocent.Count - i - 1}\n" +
                            new string('-', 50)
                        );
                    }

                    // This try block is where the actual clue selection and assertions happen.
                    try
                    {
                        Difficulties.All[diff].SelectClues(clues, characters, victim);

                        if (Debugger.IsAttached)
                        {
                            output.WriteLine($"One of the below clues should contain an owner in the following: {string.Join(", ", guilty)}");
                            foreach (Clue c in clues)
                            {
                                output.WriteLine(new string('v', 25));
                                output.WriteLine($"   Owner : {c.Owner}");
                                output.WriteLine($"  Victim : {c.Victim}");
                                output.WriteLine($"SceneUri : {c.SceneUri.LocalPath}");
                                output.WriteLine($"CleanUri : {c.CleanUri.LocalPath}");
                                output.WriteLine(new string('^', 25));
                            }
                        }

                        // -------------------------
                        // This is  where the actual assertions are made
                        // -------------------------
                        asserts(clues, characters);

                        // After the assertions, clear the clues for the next iteration.
                        clues.Clear();
                    }
                    // If an exception is thrown, meaning if any of the assertions failed, this will catch it.
                    // It's not a beautiful way to do it, and yes I know it's just "any exception,"
                    // but since the assertions happen in what is technically another method,
                    // I don't know the best way to handle it. :)
                    catch (Exception e)
                    { Assert.Fail($"Iteration {j} with {livingInnocent.Count - i - 1} innocent living characters threw an exception: {e.Message}"); }
                }
            }

            // After all iterations for this victim, reincarnate every character ;)
            foreach (Character character in characters.Values)
            { character.IsAlive = true; }
        }
    }
}