using System.Collections.Generic;

namespace SpaceTeamMystery
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