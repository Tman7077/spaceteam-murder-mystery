namespace SMM.Models.Difficulties;

/// <summary>
/// A class containing methods specific to a game on hard difficulty.
/// </summary>
public class DHard : IDifficulty
{
    // The only difference between this and the default is that it selects two guilty characters.
    public static void SelectGuilty(CharacterSet chars)
    {
        Random random = new();
        var keys = chars.Keys.ToList();
        string key1 = keys[random.Next(keys.Count)];
        keys.Remove(key1);
        string key2 = keys[random.Next(keys.Count)];

        chars[key1].IsGuilty = true;
        chars[key2].IsGuilty = true;
    }

    /// <summary>
    /// Selects clues for the easy difficulty.
    /// Assuming everyone is alive, this will select <b>all</b> clues from living innocent characters.
    /// <para>
    /// If not all characters are alive,
    /// it will select as many clues as there are living innocent characters.
    /// </para>
    /// </summary>
    /// <param name="clues">The empty set of clues to which to add.</param>
    /// <param name="chars">The current CharacterSet, containing innocence and life status.</param>
    /// <param name="victim">The name of the character for whose crime scene to select clues.</param>
    public static void SelectClues(HashSet<Clue> clues, CharacterSet chars, string victim)
    {
        Validator.ValidateShortCharacterName(victim);

        foreach (Character character in chars.Values)
        {
            if (character.IsAlive)
            { clues.Add(character.GetClue(victim)); }
        }
    }

    public static string GetResponse(Character interviewee, InterviewType type, string victim)
    {
        Validator.ValidateShortCharacterName(victim);
        return interviewee.GetResponse(new Random().Next(2) == 0, type, victim);
    }
}