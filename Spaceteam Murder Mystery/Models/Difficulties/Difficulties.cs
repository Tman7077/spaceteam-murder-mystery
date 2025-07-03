namespace SMM.Models.Difficulties;

/// <summary>
/// Contains methods specific to a game on different difficulties.
/// </summary>
public static class Difficulties
{
    /// <summary>
    /// Maps difficulty names to their methods.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, DifficultyMethods> All = new Dictionary<string, DifficultyMethods>
    {
        ["Easy"] =
            new DifficultyMethods
            (
                IDifficulty.SelectGuilty,
                DEasy.SelectClues,
                DEasy.GetResponse
            ),
        ["Medium"] =
            new DifficultyMethods
            (
                IDifficulty.SelectGuilty,
                DMedium.SelectClues,
                DMedium.GetResponse
            ),
        ["Hard"] =
            new DifficultyMethods
            (
                DHard.SelectGuilty,
                DHard.SelectClues,
                DHard.GetResponse
            )
    };
}