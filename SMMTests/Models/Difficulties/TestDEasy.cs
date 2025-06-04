namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Difficulties;
using SMM.Models.Helpers;
using Xunit.Abstractions;

public class TestDEasy(ITestOutputHelper output)
{
    ITestOutputHelper Output { get; } = output;
    public static readonly TheoryData<string> CharacterNames = TestDifficultyHelper.ShortCharacterNames;

    [Theory]
    [MemberData(nameof(CharacterNames))]
    public void TestSelectClues(string guilty)
    {
        IDifficulty easy = new DEasy();
        TestDifficultyHelper.TestSelectClues([guilty], easy, EasyAsserts, Output);
    }

    private static void EasyAsserts(HashSet<Clue> testClues, CharacterSet testChars, string[] owners)
    {
        string owner = owners[0];
        int numLivingInnocent = testChars.GetLivingNames(omitGuilty: true).Count;
        Assert.Equal(numLivingInnocent >= 3 ? 4 : numLivingInnocent + 1, testClues.Count);
        foreach (Clue clue in testClues)
        { Assert.True(File.Exists(clue.ImagePath)); }
        Assert.Contains(testClues, c => c.Owner == testChars[owner].ShortName);
    }
}