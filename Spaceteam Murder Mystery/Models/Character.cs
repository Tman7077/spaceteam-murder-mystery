namespace SMM.Models;

/// <summary>
/// Represents an entire character, including their personal information,
/// relevant image filepaths, quotes, and clues to other characters.
/// </summary>
public class Character()
{
    // --------------- About the character: static --------------- //

    /// <summary>
    /// The character's full name (first and last).
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The character's short name (just first).
    /// </summary>
    public required string ShortName { get; init; }

    /// <summary>
    /// A short (few-word) description of the character.
    /// </summary>
    public required string Role { get; init; }

    /// <summary>
    /// The character's motto or catch phrase.
    /// </summary>
    public required string Motto { get; init; }

    /// <summary>
    /// The path to the character's profile image.
    /// </summary>
    public required Uri ProfileUri { get; init; }

    /// <summary>
    /// The path to the character's profile image specific to the end screen.
    /// </summary>
    public required Uri EndProfileUri { get; init; }

    /// <summary>
    /// The path to the character's crime scene image.
    /// </summary>
    public required Uri CrimeSceneUri { get; init; }

    /// <summary>
    /// The direction that the character's portrait is facing.
    /// </summary>
    public required Direction Facing { get; init; }

    /// <summary>
    /// The position of the character's portrait on the end screen.
    /// </summary>
    public required int[] EndScreenPos { get; init; }

    /// <summary>
    /// A multi-sentence introductory description of the character.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The text to display when the character dies, introducing their crime scene.
    /// </summary>
    public required string PreDeathBlurb { get; init; }

    /// <summary>
    /// A short memorial to the character, displayed after their death.
    /// </summary>
    public required string PostDeathBlurb { get; init; }

    /// <summary>
    /// The set of clues that link this character to the others,
    /// to show in other characters' crime scenes.
    /// </summary>
    public required HashSet<Clue> Clues { get; init; }

    /// <summary>
    /// The set of responses this character gives during interviews.
    /// </summary>
    public required InterviewSet Interviews { get; init; }

    /// <summary>
    /// The set of responses this character gives upon being accused.
    /// </summary>
    public required InterviewSet Accusations { get; init; }


    // ----------------- Mutable character status ----------------- //

    /// <summary>
    /// Whether or not the character has been selected as
    /// guilty during this game.
    /// </summary>
    public bool IsGuilty { get; set; } = false;
    
    /// <summary>
    /// Whether or not the character is alive.
    /// </summary>
    public bool IsAlive { get; set; } = true;

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
    
    /// <summary>
    /// Gets the response for this character during an interview or accusation,
    /// based on whether they are acting guilty or innocent
    /// (which is decided based on the difficulty).
    /// </summary>
    /// <param name="actGuilty">Whether or not the character should return their guilty response.</param>
    /// <param name="type">Which kind of response to return.</param>
    /// <param name="victim">The name of the character about which the interviewee should speak in their response.</param>
    /// <returns>The character's response.</returns>
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