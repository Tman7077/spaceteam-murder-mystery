using SMM.Models.Helpers;
using SMM.Services;
using System.Diagnostics;
using Xunit.Abstractions;

namespace SMMTests.Services
{
    public class TestParser(ITestOutputHelper output)
    {
        ITestOutputHelper Output { get; } = output;

        // public static string ProjectRoot { get; } = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Spaceteam Murder Mystery"));
        public static TheoryData<string> MarkdownFiles
        {
            get
            {
                string assetDir = PathHelper.GetAssetDirectory();
                string mdFolder = Path.Combine(assetDir, "Text", "Characters");

                var data = new TheoryData<string>();

                foreach (var path in Directory.GetFiles(mdFolder, "*.md"))
                {
                    string characterName = Path.GetFileName(path)[..^3];
                    data.Add(characterName);
                }

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(MarkdownFiles))]
        public void ParseMarkdownFile(string characterName)
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
                Output.WriteLine($"          Name : {character.Name}");
                Output.WriteLine($"          Role : {character.Role}");
                Output.WriteLine($"         Motto : {character.Motto}");
                Output.WriteLine($"ProfileImgPath : {character.ProfileImagePath}");
                Output.WriteLine($"     CSImgPath : {character.CrimeSceneImagePath}");
                Output.WriteLine($"   Description : {character.Description}");
                Output.WriteLine($"    DeathStory : {character.DeathStory}");
                foreach (var clue in character.Clues)
                {
                    Output.WriteLine($"Clue for {clue.CharacterName}:");
                    Output.WriteLine($"          Name : {clue.Name}");
                    Output.WriteLine($"   Description : {clue.Description}");
                }
                foreach (string deadChar in character.Interviews.CharacterNames)
                {
                    Output.WriteLine($"Interview Responses ({deadChar}):");
                    Output.WriteLine($">     Innocent : {character.Interviews.GetInnocentResponse(deadChar)}");
                    Output.WriteLine($">       Guilty : {character.Interviews.GetGuiltyResponse(deadChar)}");
                }
                foreach (string deadChar in character.Accusations.CharacterNames)
                {
                    Output.WriteLine($"Accusation Responses ({deadChar}):");
                    Output.WriteLine($">     Innocent : {character.Accusations.GetInnocentResponse(deadChar)}");
                    Output.WriteLine($">       Guilty : {character.Accusations.GetGuiltyResponse(deadChar)}");
                }
            }
        }
    }
}
