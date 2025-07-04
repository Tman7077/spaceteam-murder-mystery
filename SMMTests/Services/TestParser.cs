﻿namespace SMMTests.Services;

using SMM.Models;
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
            string mdFolder = Path.Combine(AssetHelper.AssetDirectory, "Text", "Characters");

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
        Character character = Parser.ParseCharacter(characterName);

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
            Output.WriteLine($"    ProfileUri : {character.ProfileUri.LocalPath}");
            Output.WriteLine($"         CSUri : {character.CrimeSceneUri.LocalPath}");
            Output.WriteLine($"     Direction : {character.Facing}");
            Output.WriteLine($"   Description : {character.Description}");
            Output.WriteLine($" PreDeathBlurb : {character.PreDeathBlurb}");
            Output.WriteLine($"PostDeathBlurb : {character.PostDeathBlurb}");

            Output.WriteLine(new string('-', 25));
            foreach (var clue in character.Clues)
            {
                Output.WriteLine($"Clue for {clue.Victim}'s death:");
                Output.WriteLine($"       Name : {clue.Name}");
                Output.WriteLine($"Description : {clue.Description}");
                Output.WriteLine($"Coordinates : ({clue.CrimeScenePos[0]}, {clue.CrimeScenePos[1]}, {clue.CrimeScenePos[2]})");
                Output.WriteLine($"   SceneUri : {clue.SceneUri.LocalPath}");
                Output.WriteLine($"   CleanUri : {clue.CleanUri.LocalPath}\n");
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
        Assert.NotEmpty(story.Victory);
        Assert.NotEmpty(story.Defeat);
        Output.WriteLine("Story parsed successfully.");
        if (Debugger.IsAttached)
        {
            Output.WriteLine(new string('-', 50));
            Output.WriteLine("Debugging is attached, printing story data:");
            Output.WriteLine($"       Intro : {story.Intro}\n");
            Output.WriteLine($"First Murder : {story.FirstMurder}\n");
            Output.WriteLine($"     Victory : {story.Victory}\n");
            Output.WriteLine($"      Defeat : {story.Defeat}");
        }
    }
}
