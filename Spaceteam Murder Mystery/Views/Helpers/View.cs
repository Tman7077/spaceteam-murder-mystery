namespace SMM.Views.Helpers;

/// <summary>
/// Translates Screen records to methods that instantiate UserControls.
/// </summary>
public static class View
{
    public static Func<UserControl> Request(MainWindow window, Screen screen)
    {
        return screen switch
        {
            Screen.CrimeScene     s =>
                () => new CrimeSceneScreen(window, s.VictimName),
            Screen.CharacterIntro s =>
                () => new CharacterIntroScreen(window, s.Name, s.Index),
            Screen.Difficulty       =>
                () => new DifficultyScreen(window),
            Screen.End            s =>
                () => new EndScreen(window, s.Victory),
            Screen.InspectChar    s =>
                () => new InterviewScreen(window, s.Type, s.Interviewee, s.Victim),
            Screen.InspectClue    s =>
                () => new ClueScreen(window, s.Clue),
            Screen.Pause            =>
                () => new PauseScreen(window),
            Screen.Selection      s =>
                () => new CharacterSelectionScreen(window, s.Type),
            Screen.Settings         =>
                () => new SettingsScreen(window),
            Screen.Story          s =>
                () => new StoryScreen(window, s.Advance),
            Screen.Title            =>
                () => new TitleScreen(window),
            _ => throw new ArgumentException($"Unknown view {screen}.", nameof(screen))
        };
    }
}