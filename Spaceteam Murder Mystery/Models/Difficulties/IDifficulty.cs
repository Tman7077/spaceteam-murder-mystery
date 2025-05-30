namespace SMM.Models.Difficulties;

public interface IDifficulty
{
    void SelectGuilty(Dictionary<string, Character> chars)
    {
        string key = chars.Keys.ElementAt(new Random().Next(chars.Count));
        chars[key].IsSuspect = true;
    }
    void SelectClues();
}