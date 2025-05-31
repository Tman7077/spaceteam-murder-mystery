namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public class DMedium : IDifficulty
{
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> others = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        foreach (string guiltyChar in guilty)
        {
            bool chance = r.Next(0, 4) != 0; // 75% chance to include a guilty clue
            if (chance) clues.Add(chars[guiltyChar].GetClue(victim));
        }

        int num = (others.Count > 5) ? 5 : others.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, others.Count);
            string name = others[index];
            others.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}