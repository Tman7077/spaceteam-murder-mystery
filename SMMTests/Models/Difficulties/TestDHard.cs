namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using Xunit.Abstractions;

public class TestDHard(ITestOutputHelper output)
{
    ITestOutputHelper Output { get; } = output;
    public static readonly TheoryData<string> CharacterNames = TestDifficultyHelper.ShortCharacterNames;

    [Theory]
    [MemberData(nameof(CharacterNames))]
    public void TestSelectClues(string guilty)
    {
        IDifficulty hard = new DHard();

        HashSet<string[]> pairs = [];
        foreach (string accomplice in CharacterNames)
        { pairs.Add([guilty, accomplice]); }

        foreach (string[] pair in pairs)
        { TestDifficultyHelper.TestSelectClues(pair, hard, HardAsserts, Output); }
    }

    private static void HardAsserts(HashSet<Clue> testClues, CharacterSet testChars, string[] owners)
    {
        int numLivingInnocent = testChars.GetLivingNames(omitGuilty: true).Count;
        int minClues = numLivingInnocent + 1;
        int maxClues = minClues + 2;
        Assert.InRange(testClues.Count, minClues, maxClues);
        foreach (Clue clue in testClues)
        { Assert.True(File.Exists(clue.ImagePath)); }
    }
}