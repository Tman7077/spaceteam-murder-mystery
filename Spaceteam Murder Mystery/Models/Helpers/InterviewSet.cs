namespace SMM.Models.Helpers;

/// <summary>
/// A collection of responses to interviews/accusations.
/// Name strings are mapped to response sets.
/// </summary>
public class InterviewSet
{
    private readonly Dictionary<string, ResponseSet> _responses = [];
    
    public IEnumerable<string> CharacterNames => _responses.Keys;

    /// <summary>
    /// Adds a new character's responses to the interview set.
    /// </summary>
    /// <param name="characterName">The name of the character (victim) about whom to add responses.</param>
    /// <param name="responses">The responses to be added.</param>
    public void Add(string characterName, ResponseSet responses)
    {
        if (_responses.ContainsKey(characterName))
        { throw new ArgumentException($"Character {characterName} already exists in the interview set."); }
        _responses[characterName] = responses;
    }

    /// <summary>
    /// Gets the innocent response for a character in the interview set.
    /// </summary>
    /// <param name="characterName">The character (victim) for whom to retrieve a response.</param>
    /// <returns>The response for the given character.</returns>
    public string GetInnocentResponse(string characterName) =>
        Get(characterName).Innocent;

    /// <summary>
    /// Gets the guilty response for a character in the interview set.
    /// </summary>
    /// <param name="characterName">The character (victim) for whom to retrieve a response.</param>
    /// <returns>The response for the given character.</returns>
    public string GetGuiltyResponse(string characterName) =>
        Get(characterName).Guilty;
    
    private ResponseSet Get(string characterName)
    {
        if (!_responses.TryGetValue(characterName, out var responses))
        { throw new KeyNotFoundException($"Character {characterName} not found in the interview set."); }
        return responses;
    }
}