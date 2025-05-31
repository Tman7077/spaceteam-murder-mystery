namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public class DEasy : IDifficulty
{
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> others = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        foreach (string guiltyChar in guilty)
        { clues.Add(chars[guiltyChar].GetClue(victim)); }

        int num = (others.Count > 3) ? 3 : others.Count;
        for (int i = 0; i < num; i++)
        {
            int index = r.Next(0, others.Count);
            string name = others[index];
            others.Remove(name);
            clues.Add(chars[name].GetClue(victim));
        }
    }
}