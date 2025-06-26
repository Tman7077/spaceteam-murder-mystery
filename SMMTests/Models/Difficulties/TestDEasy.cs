namespace SMMTests.Models.Difficulties;

using SMM.Models;
using SMM.Models.Helpers;
using Xunit.Abstractions;

public class TestDEasy(ITestOutputHelper output)
{
    ITestOutputHelper Output { get; } = output;
    public static readonly TheoryData<string> CharacterNames = TestDifficultyHelper.ShortCharacterNames;

    [Theory]
    [MemberData(nameof(CharacterNames))]
    public void TestSelectClues(string guilty) =>
        TestDifficultyHelper.TestSelectClues([guilty], "Easy", EasyAsserts, Output);

    /// <summary>
    /// This method contains the assertions for <i>TestSelectClues</i> above.
    /// The number of living characters determines the number of clues that should be selected,
    /// and the clues should all contain valid image paths; also
    /// at least one clue should be owned by the guilty character.
    /// </summary>
    /// <param name="testClues">A set of clues passed that should meet certain requirements.</param>
    /// <param name="testChars">A set of characters used to determine what requirements <b>testClues</b> must meet.</param>
    private static void EasyAsserts(HashSet<Clue> testClues, CharacterSet testChars)
    {
        string owner = testChars.GetGuiltyNames()[0];
        int numLivingInnocent = testChars.GetLivingNames(includeGuilty: false).Count;
        // The number of clues is either:
        // 4 (guilty character), or
        // the number of living innocents + 1 (for the guilty character).
        Assert.Equal(numLivingInnocent >= 3 ? 4 : numLivingInnocent + 1, testClues.Count);
        // Ensure all clues have valid image paths.
        foreach (Clue clue in testClues)
        { Assert.True(File.Exists(clue.Uri.LocalPath)); }
        Assert.Contains(testClues, c => c.Owner == testChars[owner].ShortName);
    }
}