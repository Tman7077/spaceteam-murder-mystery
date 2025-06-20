namespace SMM;

using System.ComponentModel;

public partial class MainWindow : Window
{
    private readonly WindowHandler _windowHandler;
    private UserControl? _prevScreen;
    private GameState? _gameState;

    public GameState State
    {
        get => _gameState ?? throw new InvalidOperationException("GameState has not yet been initialized");
        private set => _gameState = value;
    }

    public MainWindow()
    {
        InitializeComponent();

        _windowHandler = new(this);

        bool fullscreen = AppSettings.Load(_windowHandler);
        if (fullscreen)
        { SourceInitialized += ImmediateFullScreen; }

        Activated += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        ChangeView(new Screen.Title());
    }

    public void StartGame(string difficulty)
    {
        if (!Difficulties.All.ContainsKey(difficulty))
        { throw new ArgumentException($"Unknown difficulty: {difficulty}"); }

        State = new GameState(difficulty);
        ChangeView(new Screen.NewGame());
    }

    public void ChangeView(Screen screen)
    {
        _prevScreen = View.Request(this, screen)();
        MainContent.Content = _prevScreen;
    }

    public void LoadCrimeSceneFor(string victimName) =>
        ChangeView(new Screen.CrimeScene(victimName));

    public void LoadInterviewFor(string interviewee) =>
        ChangeView(new Screen.InspectionChar(InterviewType.Interview, interviewee, State.LastVictim));

    public void LoadAccusationFor(string interviewee) =>
        ChangeView(new Screen.InspectionChar(InterviewType.Accusation, interviewee, State.LastVictim));

    public void LoadClueInspectionFor(Clue clue) =>
        ChangeView(new Screen.InspectionClue(clue));

    private void ImmediateFullScreen(object? sender, EventArgs e) =>
        _windowHandler.ToggleFullScreen();

    public void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        { _windowHandler.ToggleFullScreen(); }
    }
    
    protected override void OnClosing(CancelEventArgs e)
    {
        AppSettings.Save(_windowHandler);
        base.OnClosing(e);
    }
}