namespace  SMM.Models.Difficulties;

public static class Difficulties
{
    public static readonly IReadOnlyDictionary<string, DifficultyMethods> All = new Dictionary<string, DifficultyMethods>
    {
        ["Easy"] =   new DifficultyMethods(IDifficulty.SelectGuilty, DEasy.SelectClues),
        ["Medium"] = new DifficultyMethods(IDifficulty.SelectGuilty, DMedium.SelectClues),
        ["Hard"] =   new DifficultyMethods(      DHard.SelectGuilty, DHard.SelectClues)
    };
}