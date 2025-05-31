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
        List<string> others = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        foreach (string guiltyChar in guilty)
        {
            bool chance = r.Next(0, 2) != 0; // 50% chance to include a guilty clue
            if (chance) clues.Add(chars[guiltyChar].GetClue(victim));
        }

        int num = others.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, others.Count);
            string name = others[index];
            others.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}