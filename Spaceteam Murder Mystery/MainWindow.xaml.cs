namespace SMM;

using System.ComponentModel;

public partial class MainWindow : Window
{
    private readonly WindowHandler _windowHandler;
    private readonly Stack<UserControl> _viewHistory = [];
    private GameState? _gameState;

    private UserControl PrevScreen
    {
        get => _viewHistory.Count > 0
            ? _viewHistory.Pop()
            : throw new IndexOutOfRangeException("No previous screen available");
        set => _viewHistory.Push(value);
    }
    private UserControl CurrentScreen
    {
        get => Content as UserControl
            ?? throw new InvalidOperationException("MainContent is not set to a UserControl");
        set => Content = value;
    }
    public GameState State
    {
        get => _gameState
            ?? throw new InvalidOperationException("GameState has not yet been initialized");
        private set => _gameState = value;
    }

    public MainWindow()
    {
        InitializeComponent();

        _windowHandler = new(this);

        AppSettings.Load(_windowHandler, out bool fullscreen);
        if (fullscreen)
        { SourceInitialized += ImmediateFullScreen; }

        Activated   += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        CurrentScreen = View.Request(this, new Screen.Title())();
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
        PrevScreen = CurrentScreen;
        CurrentScreen = View.Request(this, screen)();
    }
    public void ToPreviousScreen() => CurrentScreen = PrevScreen;

    public void LoadCrimeSceneFor(string victimName) =>
        ChangeView(new Screen.CrimeScene(victimName));

    public void LoadInterviewFor(string interviewee) =>
        ChangeView(new Screen.InspectChar(InterviewType.Interview, interviewee, State.LastVictim));

    public void LoadAccusationFor(string interviewee) =>
        ChangeView(new Screen.InspectChar(InterviewType.Accusation, interviewee, State.LastVictim));

    public void LoadClueInspectionFor(Clue clue) =>
        ChangeView(new Screen.InspectClue(clue));

    public void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        { _windowHandler.ToggleFullScreen(); }
    }
    
    private void ImmediateFullScreen(object? sender, EventArgs e) =>
        _windowHandler.ToggleFullScreen();
    
    protected override void OnClosing(CancelEventArgs e)
    {
        AppSettings.Save(_windowHandler);
        base.OnClosing(e);
    }
}