namespace SMM.Models.Helpers;

public class InterviewSet
{
    private readonly Dictionary<string, ResponseSet> _responses = [];
    public IEnumerable<string> CharacterNames => _responses.Keys;

    public void Add(string characterName, ResponseSet responses)
    {
        if (_responses.ContainsKey(characterName))
        {
            throw new ArgumentException($"Character {characterName} already exists in the interview set.");
        }
        _responses[characterName] = responses;
    }
    public string GetInnocentResponse(string characterName)
    {
        return Get(characterName).Innocent;
    }
    public string GetGuiltyResponse(string characterName)
    {
        return Get(characterName).Guilty;
    }
    private ResponseSet Get(string characterName)
    {
        if (_responses.TryGetValue(characterName, out var responses))
        {
            return responses;
        }
        throw new KeyNotFoundException($"Character {characterName} not found in the interview set.");
    }
}