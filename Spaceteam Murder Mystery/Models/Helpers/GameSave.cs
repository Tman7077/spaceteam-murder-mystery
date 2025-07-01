namespace SMM.Models.Helpers;

using MessagePack;

[MessagePackObject]
public class GameSave
{
    [Key(0)] // character: living, guilty
    public required Dictionary<string, (bool, bool)> CharacterData { get; set; }
    [Key(1)]
    public required string    Difficulty      { get; set; }
    [Key(2)]
    public required string    LastVictim      { get; set; }
    [Key(3)]
    public required string    LastViewName    { get; set; }
    [Key(4)]
    public string[]?          CrimeSceneData  { get; set; }
    [Key(5)]
    public (string, string?)? StoryScreenData { get; set; }
}