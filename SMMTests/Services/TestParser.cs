namespace SMMTests.Services;

using SMM.Models;
using SMM.Models.Helpers;
using SMM.Services;
using System.Diagnostics;
using Xunit.Abstractions;

public class TestParser(ITestOutputHelper output)
{
    ITestOutputHelper Output { get; } = output;

    public static TheoryData<string> MarkdownFiles
    {
        get
        {
            var data = new TheoryData<string>();
            string assetDir = PathHelper.GetAssetDirectory();
            string mdFolder = Path.Combine(assetDir, "Text", "Characters");

            foreach (var charFile in Directory.GetFiles(mdFolder, "*.md"))
            {
                string characterName = Path.GetFileNameWithoutExtension(charFile);
                data.Add(characterName);
            }

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(MarkdownFiles))]
    public void TestParseMDFile(string characterName)
    {
        // Confirm that the character name is at least right
        // This will come from reading the files in the directory
        Output.WriteLine($"Loaded Character: {characterName}");
        CharacterData character = Parser.ParseCharacter(characterName);

        // If at least the name was read correctly, this will pass.
        Assert.Contains(characterName, character.Name);

        if (Debugger.IsAttached)
        {
            // This is just for manual verification:
            // It will print out the character data to the console
            // so you can see if it looks right.
            Output.WriteLine(new string('-', 50));
            Output.WriteLine("Debugging is attached, printing character data:");

            Output.WriteLine($"          Name : {character.Name}");
            Output.WriteLine($"          Role : {character.Role}");
            Output.WriteLine($"         Motto : {character.Motto}");
            Output.WriteLine($"ProfileImgPath : {character.ProfileImagePath}");
            Output.WriteLine($"     CSImgPath : {character.CrimeSceneImagePath}");
            Output.WriteLine($"   Description : {character.Description}");
            Output.WriteLine($"    DeathStory : {character.DeathStory}");

            Output.WriteLine(new string('-', 25));
            foreach (var clue in character.Clues)
            {
                Output.WriteLine($"Clue for {clue.Victim}'s death:");
                Output.WriteLine($"       Name : {clue.Name}");
                Output.WriteLine($"Description : {clue.Description}");
                Output.WriteLine($"Coordinates : ({clue.X}, {clue.Y}, {clue.Z})");
                Output.WriteLine($" Image Path : {clue.ImagePath}\n");
            }

            Output.WriteLine(new string('-', 25));
            foreach (string deadChar in character.Interviews.CharacterNames)
            {
                Output.WriteLine($"Interview Responses ({deadChar}):");
                Output.WriteLine($"> Innocent : {character.Interviews.GetInnocentResponse(deadChar)}");
                Output.WriteLine($">   Guilty : {character.Interviews.GetGuiltyResponse(deadChar)}\n");
            }

            Output.WriteLine(new string('-', 25));
            foreach (string deadChar in character.Accusations.CharacterNames)
            {
                Output.WriteLine($"Accusation Responses ({deadChar}):");
                Output.WriteLine($"> Innocent : {character.Accusations.GetInnocentResponse(deadChar)}");
                Output.WriteLine($">   Guilty : {character.Accusations.GetGuiltyResponse(deadChar)}\n");
            }
        }
    }

    [Fact]
    public void TestParseStory()
    {
        Story story = Parser.ParseStoryData();
        Assert.NotNull(story);
        Assert.NotEmpty(story.Intro);
        Assert.NotEmpty(story.FirstMurder);
        Output.WriteLine("Story parsed successfully.");
        if (Debugger.IsAttached)
        {
            Output.WriteLine(new string('-', 50));
            Output.WriteLine("Debugging is attached, printing story data:");
            Output.WriteLine($"       Intro : {story.Intro}\n");
            Output.WriteLine($"First Murder : {story.FirstMurder}");
        }
    }
}
