namespace SMM.Models.Difficulties;

/// <summary>
/// A class containing methods specific to a game on easy difficulty.
/// </summary>
public class DEasy : IDifficulty
{
    /// <summary>
    /// Selects clues for the easy difficulty.
    /// Assuming everyone is alive, this will select:
    /// <para>
    /// • <b>3</b> clues from random living innocent characters and
    /// </para>
    /// <para>
    /// • <b>1</b> clue from each guilty character.
    /// </para>
    /// <para>
    /// If there are less than 3 living innocent characters,
    /// it will select as many clues as there are living innocent characters
    /// (in addition to the guilty character's clue).
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

        // Add the guilty characters' clues.
        foreach (string guiltyChar in guilty)
        { clues.Add(chars[guiltyChar].GetClue(victim)); }

        // Add up to 3 clues from living innocent characters.
        int num = (livingInnocent.Count >= 3) ? 3 : livingInnocent.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, livingInnocent.Count);
            string name = livingInnocent[index];
            livingInnocent.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}