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
    /// Assuming everyone is alive, this will select:
    /// <para>
    /// • <b>All</b> clues from living innocent characters;
    /// </para>
    /// <para>
    /// • <b>1</b> clue from the victim's own character; and
    /// </para>
    /// <para>
    /// • <b>1</b> clue from each guilty character <i>(with a 50% chance each)</i>.
    /// </para>
    /// <para>
    /// If not all characters are alive,
    /// it will select as many clues as there are living innocent characters
    /// (in addition to the random possible selections above).
    /// </para>
    /// </summary>
    /// <param name="clues">The empty set of clues to which to add.</param>
    /// <param name="chars">The current CharacterSet, containing innocence and life status.</param>
    /// <param name="victim">The name of the character for whose crime scene to select clues.</param>
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> livingInnocent = chars.GetLivingNames(includeGuilty: false);
        Random r = new();

        // Add the victim's own clue into the mix
        clues.Add(chars[victim].GetClue(victim));

        foreach (string guiltyChar in guilty)
        {
            // 50% chance to include a guilty clue
            bool maybe = r.Next(0, 2) != 0;
            if (maybe) { clues.Add(chars[guiltyChar].GetClue(victim)); }
        }

        // Add all clues from living innocent characters.
        int num = livingInnocent.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, livingInnocent.Count);
            string name = livingInnocent[index];
            livingInnocent.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}