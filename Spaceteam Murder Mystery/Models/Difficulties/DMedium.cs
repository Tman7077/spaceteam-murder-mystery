namespace SMM.Models.Difficulties;

/// <summary>
/// A class containing methods specific to a game on medium difficulty.
/// </summary>
public class DMedium : IDifficulty
{
    /// <summary>
    /// Selects clues for the easy difficulty.
    /// Assuming everyone is alive, this will select:
    /// <para>
    /// • <b>4</b> clues from random living innocent characters;
    /// </para>
    /// <para>
    /// • <b>1</b> clue from the victim's own character <i>(with a 50% chance)</i>; and
    /// </para>
    /// <para>
    /// • <b>1</b> clue from each guilty character <i>(with a 75% chance each)</i>.
    /// </para>
    /// <para>
    /// If there are less than 4 living innocent characters,
    /// it will select as many clues as there are living innocent characters
    /// (in addition to the random possible selections above).
    /// </para>
    /// </summary>
    /// <param name="clues">The empty set of clues to which to add.</param>
    /// <param name="chars">The current CharacterSet, containing innocence and life status.</param>
    /// <param name="victim">The name of the character for whose crime scene to select clues.</param>
    public static void SelectClues(HashSet<Clue> clues, CharacterSet chars, string victim)
    {
        Validator.ValidateShortCharacterName(victim);

        List<string> guilty = chars.GetGuiltyNames();
        List<string> livingInnocent = chars.GetLivingNames(includeGuilty: false);
        Random r = new();

        // 50% chance to add the victim's own clue into the mix
        bool maybe = r.Next(0, 2) == 0;
        if (maybe) { clues.Add(chars[victim].GetClue(victim)); }

        foreach (string guiltyChar in guilty)
        {
            // 75% chance to include a guilty clue
            bool probably = r.Next(0, 4) != 0;
            if (probably) { clues.Add(chars[guiltyChar].GetClue(victim)); }
        }

        // Add up to 4 clues from living innocent characters.
        int num = (livingInnocent.Count >= 4) ? 4 : livingInnocent.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, livingInnocent.Count);
            string name = livingInnocent[index];
            livingInnocent.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }

    public static string GetResponse(Character interviewee, InterviewType type, string victim)
    {
        Validator.ValidateShortCharacterName(victim);
        
        bool flip = new Random().Next(3) == 0;
        bool actGuilty = interviewee.IsGuilty ^ flip; // 1/3 chance to rspond with wrong guilt state
        return interviewee.GetResponse(actGuilty, type, victim);
    }
}