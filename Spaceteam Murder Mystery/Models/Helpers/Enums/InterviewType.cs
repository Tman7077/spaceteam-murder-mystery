namespace SMM.Models.Helpers.Enums;

/// <summary>
/// Represents whether the interview to display
/// should be informational or accusatory.
/// </summary>
public enum InterviewType : byte
{
    /// <summary>
    /// Used to get a response for interviewing after
    /// seeing a clue but before making an accusation.
    /// </summary>
    Interview,

    /// <summary>
    /// Used to get a response for accusing a player
    /// of committing a murder.
    /// </summary>
    Accusation
}