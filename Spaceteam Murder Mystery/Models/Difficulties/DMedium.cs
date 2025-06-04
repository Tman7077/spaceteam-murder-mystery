namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public class DMedium : IDifficulty
{
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> livingInnocent = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        bool maybe = r.Next(0, 2) == 0; // 50% chance to add the victim's own clue into the mix
        if (maybe) clues.Add(chars[victim].GetClue(victim));

        foreach (string guiltyChar in guilty)
        {
            bool probably = r.Next(0, 4) != 0; // 75% chance to include a guilty clue
            if (probably) { clues.Add(chars[guiltyChar].GetClue(victim)); }
        }

        int num = (livingInnocent.Count >= 4) ? 4 : livingInnocent.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, livingInnocent.Count);
            string name = livingInnocent[index];
            livingInnocent.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}