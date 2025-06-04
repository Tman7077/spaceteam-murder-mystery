namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public class DEasy : IDifficulty
{
    public void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim)
    {
        List<string> guilty = chars.GetGuiltyNames();
        List<string> livingInnocent = chars.GetLivingNames(omitGuilty: true);
        Random r = new();

        foreach (string guiltyChar in guilty)
        { clues.Add(chars[guiltyChar].GetClue(victim)); }

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