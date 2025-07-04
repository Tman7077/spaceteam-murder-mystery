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
        if (CharacterNames.Contains(characterName))
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
    
    /// <summary>
    /// Gets the response set for a character in the interview set.
    /// </summary>
    /// <param name="characterName">The name of the character about which to speak.</param>
    /// <returns>The ResponseSet containing the innocent and guilty responses about this character.</returns>
    private ResponseSet Get(string characterName)
    {
        Validator.ValidateShortCharacterName(characterName);

        if (!_responses.TryGetValue(characterName, out var responses))
        { throw new KeyNotFoundException($"Character {characterName} not found in the interview set."); }
        return responses;
    }
}