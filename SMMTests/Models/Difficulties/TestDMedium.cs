namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using Xunit.Abstractions;

public class TestDMedium(ITestOutputHelper output)
{
    ITestOutputHelper Output { get; } = output;
    public static readonly TheoryData<string> CharacterNames = TestDifficultyHelper.ShortCharacterNames;

    [Theory]
    [MemberData(nameof(CharacterNames))]
    public void TestSelectClues(string guilty)
    {
        IDifficulty medium = new DMedium();
        TestDifficultyHelper.TestSelectClues([guilty], medium, MediumAsserts, Output);
    }

    private static void MediumAsserts(HashSet<Clue> testClues, CharacterSet testChars, string[] owners)
    {
        int numLivingInnocent = testChars.GetLivingNames(omitGuilty: true).Count;
        int minClues = numLivingInnocent >= 4 ? 4 : numLivingInnocent;
        int maxClues = minClues + 3;
        Assert.InRange(testClues.Count, minClues, maxClues);
        foreach (Clue clue in testClues)
        { Assert.True(File.Exists(clue.ImagePath)); }
    }
}