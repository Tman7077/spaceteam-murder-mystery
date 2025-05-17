using System.Collections.Generic;

namespace SMM
{
    public static class GameState
    {
        public static HashSet<string> FoundClues = new();
        public static string CurrentScene = "Intro";

        public static void Reset()
        {
            FoundClues.Clear();
            CurrentScene = "Intro";
        }
    }
}