namespace SMM.Models.Difficulties;

using SMM.Models.Helpers;

public interface IDifficulty
{
    void SelectGuilty(CharacterSet chars)
    {
        string key = chars.Keys.ElementAt(new Random().Next(chars.Count));
        chars[key].IsGuilty = true;
    }
    void SelectClues(ref HashSet<Clue> clues, ref CharacterSet chars, string victim);
}