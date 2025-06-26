namespace SMM.Views.Helpers;

public static class View
{
    public static Func<UserControl> Request(MainWindow window, Screen screen)
    {
        return screen switch
        {
            Screen.CrimeScene  s =>
                () => new CrimeSceneScreen(window, s.VictimName),
            Screen.Difficulty    =>
                () => new DifficultyScreen(window),
            Screen.InspectChar s =>
                () => new InterviewScreen(window, s.Type, s.Interviewee, s.Victim),
            Screen.InspectClue s =>
                () => new ClueScreen(window, s.Clue),
            Screen.Selection   s =>
                () => new CharacterSelectionScreen(window, s.Type),
            Screen.Settings      =>
                () => new SettingsScreen(window),
            Screen.Story       s =>
                () => new StoryScreen(window, s.FirstLoad, s.Vote),
            Screen.Success     s =>
                () => new SuccessScreen(window, s.Vote),
            Screen.Title         =>
                () => new TitleScreen(window),
            _ => throw new ArgumentException($"Unknown view {screen}.", nameof(screen))
        };
    }
}