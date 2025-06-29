namespace SMM.Models;

/// <summary>
/// Represents an entire character, including their personal information,
/// relevant image filepaths, quotes, and clues to other characters.
/// </summary>
public class Character()
{
    // About the character: static
    public required string        Name           { get; init; }
    public required string        ShortName      { get; init; }
    public required string        Role           { get; init; }
    public required string        Motto          { get; init; }
    public required Uri           ProfileUri     { get; init; }
    public required Uri           EndProfileUri  { get; init; }
    public required Uri           CrimeSceneUri  { get; init; }
    public required Direction     Facing         { get; init; }
    public required int[]         EndScreenPos   { get; init; }
    public required string        Description    { get; init; }
    public required string        PreDeathBlurb  { get; init; }
    public required string        PostDeathBlurb { get; init; }
    public required HashSet<Clue> Clues          { get; init; }
    public required InterviewSet  Interviews     { get; init; }
    public required InterviewSet  Accusations    { get; init; }

    // Mutable character status: can change mid-game.
    public bool IsGuilty { get; set; } = false;
    public bool IsAlive  { get; set; } = true;

    /// <summary>
    /// Gets the Clue implicating this character in the death of the given victim.
    /// </summary>
    /// <param name="victim">The name of the victim for whom to retrieve a clue.</param>
    /// <returns>The relevant clue.</returns>
    public Clue GetClue(string victim)
    {
        Validator.ValidateShortCharacterName(victim);

        return Clues.FirstOrDefault(clue => clue.Victim == victim)
            ?? throw new InvalidOperationException($"No clue found for victim '{victim}' in character '{ShortName}'.");
    }
    
    public string GetResponse(bool actGuilty, InterviewType type, string victim)
    {
        Validator.ValidateShortCharacterName(victim);

        return (actGuilty, type) switch
        {
            (false, InterviewType.Interview)  => Interviews.GetInnocentResponse(victim),
            (true,  InterviewType.Interview)  => Interviews.GetGuiltyResponse(victim),
            (false, InterviewType.Accusation) => Accusations.GetInnocentResponse(victim),
            (true,  InterviewType.Accusation) => Accusations.GetGuiltyResponse(victim),
            _ => throw
                new ArgumentException(
                    $"Invalid interviewee (act) guilty state or interview type. " +
                    $"{ShortName}, {actGuilty}, {type}"
                ),
        };
    }
}