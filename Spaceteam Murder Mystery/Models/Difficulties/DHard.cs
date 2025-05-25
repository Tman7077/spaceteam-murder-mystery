namespace SMM.Models.Difficulties
{
    public class DHard : IDifficulty
    {
        public static void SelectGuilty(Dictionary<string, Character> chars)
        {
            Random random = new();
            string key1 = chars.Keys.ElementAt(random.Next(chars.Count));
            chars[key1].IsSuspect = true;
            string key2 = chars.Keys.ElementAt(random.Next(chars.Count));
            chars[key2].IsSuspect = true;
        }
        public void SelectClues()
        {
            // Something here
        }
    }
}