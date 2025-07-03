namespace SMM.Models.Helpers;

/// <summary>
/// A wrapper for a tuple of the form (innocent response, guilty response).
/// <para>
/// The purpose is to provide a structure with named properties
/// for the responses of a character in an interview.
/// </para>
/// </summary>
/// <param name="innocent">The response if the interviewee is innocent.</param>
/// <param name="guilty">The response if the interviewee is guilty.</param>
public class ResponseSet(string innocent, string guilty)
{
    /// <summary>
    /// The response if the interviewee is innocent.
    /// </summary>
    public string Innocent { get; } = innocent;

    /// <summary>
    /// The response if the interviewee is guilty.
    /// </summary>
    public string Guilty { get; } = guilty;
}