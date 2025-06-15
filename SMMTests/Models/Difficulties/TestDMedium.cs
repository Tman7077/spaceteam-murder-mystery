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
        TestDifficultyHelper.TestSelectClues([guilty], "Medium", MediumAsserts, Output);
    }

    /// <summary>
    /// This method contains the assertions for <i>TestSelectClues</i> above.
    /// The number of living characters determines the number of clues that should be selected,
    /// and the clues should all contain valid image paths.
    /// </summary>
    /// <param name="testClues">A set of clues passed that should meet certain requirements.</param>
    /// <param name="testChars">A set of characters used to determine what requirements <b>testClues</b> must meet.</param>
    private static void MediumAsserts(HashSet<Clue> testClues, CharacterSet testChars)
    {
        int numLivingInnocent = testChars.GetLivingNames(includeGuilty: false).Count;
        // The number of clues is randomized, but must be between
        // (the number of living innocents + 1)
        int minClues = numLivingInnocent >= 4 ? 4 : numLivingInnocent;
        int maxClues = minClues + 3;
        Assert.InRange(testClues.Count, minClues, maxClues);
        // Ensure all clues have valid image paths.
        foreach (Clue clue in testClues)
        { Assert.True(File.Exists(clue.ImagePath)); }
    }
}