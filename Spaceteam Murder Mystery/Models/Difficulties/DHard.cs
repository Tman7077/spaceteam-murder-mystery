namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public class DHard : IDifficulty
{
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
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> livingInnocent = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        clues.Add(chars[victim].GetClue(victim)); // Add the victim's own clue into the mix

        foreach (string guiltyChar in guilty)
        {
            bool maybe = r.Next(0, 2) != 0; // 50% chance to include a guilty clue
            if (maybe) { clues.Add(chars[guiltyChar].GetClue(victim)); }
        }

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