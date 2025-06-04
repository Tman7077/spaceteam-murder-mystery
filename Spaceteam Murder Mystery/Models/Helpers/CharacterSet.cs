namespace SMM.Models.Helpers;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// A wrapper for a dictionary of strings and Characters.
/// Typical properties and methods for dictionaries are exposed,
/// but some additional methods are available.
/// </summary>
public class CharacterSet
{
    private readonly Dictionary<string, Character> _characters = [];

    // Typical Dictionary properties and methods
    // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
    public IEnumerable<string> Keys => _characters.Keys;
    public IEnumerable<Character> Values => _characters.Values;
    public int Count => _characters.Count;

    public Character this[string key]
    {
        get => _characters[key];
        set => _characters[key] = value;
    }

    // I don't think I need this one :).
    // public void Add(string key, Character character) => _characters.Add(key, character);
    public void Clear() => _characters.Clear();
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Character character) => _characters.TryGetValue(key, out character);
    public bool ContainsKey(string key) => _characters.ContainsKey(key);
    // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

    /// <summary>
    /// Gets a list of names of characters who are guilty.
    /// </summary>
    /// <param name="includeDead">Whether or not to include the names of characters who are dead.</param>
    /// <returns>A list of names of guilty characters.</returns>
    public List<string> GetGuiltyNames(bool includeDead = false)
    {
        List<string> guilty = [];
        foreach (string name in _characters.Keys)
        {
            Character character = _characters[name];
            if (character.IsGuilty && (character.IsAlive || includeDead))
                guilty.Add(name);
        }
        return guilty;
    }
    
    /// <summary>
    /// Gets a list of names of characters who are alive.
    /// </summary>
    /// <param name="includeGuilty">Whether or not to include names of characters who are guilty.</param>
    /// <returns>A list of names of living characters.</returns>
    public List<string> GetLivingNames(bool includeGuilty = true)
    {
        List<string> living = [];
        foreach (string name in _characters.Keys)
        {
            Character character = _characters[name];
            if (character.IsAlive && (!character.IsGuilty || includeGuilty))
                living.Add(name);
        }
        return living;
    }
}