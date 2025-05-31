namespace SMM.Models.Helpers;

using System.Diagnostics.CodeAnalysis;

public class CharacterSet
{
    private readonly Dictionary<string, Character> _characters = [];
    public IEnumerable<string> Keys => _characters.Keys;
    public IEnumerable<Character> Values => _characters.Values;
    public int Count => _characters.Count;

    public Character this[string key]
    {
        get => _characters[key];
        set => _characters[key] = value;
    }

    public void Add(string key, Character character) => _characters.Add(key, character);
    public void Clear() => _characters.Clear();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Character character) => _characters.TryGetValue(key, out character);

    public bool ContainsKey(string key) => _characters.ContainsKey(key);

    public string[] GetGuiltyNames(bool onlyLiving = true)
    {
        List<string> guilty = [];
        foreach (string name in _characters.Keys)
        {
            Character character = _characters[name];
            if (character.IsGuilty && (!onlyLiving || character.IsAlive))
                guilty.Add(name);
        }
        return [.. guilty];
    }
    public string[] GetLivingNames()
    {
        List<string> living = [];
        foreach (string name in _characters.Keys)
        {
            Character character = _characters[name];
            if (character.IsAlive)
                living.Add(name);
        }
        return [.. living];
    }
}