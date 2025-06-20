namespace SMM;

using System.ComponentModel;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, Func<UserControl>> _viewMap;
    private readonly WindowHandler _windowHandler;
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

        Activated   += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        _viewMap = new()
        {
            { "Title",      () => new TitleScreen(this) },
            { "Settings",   () => new SettingsScreen(this) },
            { "Difficulty", () => new DifficultyScreen(this) },
            { "NewGame",    () => new GameScreen(this) },
            { "Interview",  () => new CharacterSelectionScreen(this, InterviewType.Interview) },
            { "Accusation", () => new CharacterSelectionScreen(this, InterviewType.Accusation) },
            { "Voting",     () => new CharacterSelectionScreen(this, "Voting") }
        };

        ChangeView("Title");
    }

    public void StartGame(string difficulty)
    {
        if (!Difficulties.All.ContainsKey(difficulty))
        { throw new ArgumentException($"Unknown difficulty: {difficulty}"); }

        State = new GameState(difficulty);
        ChangeView("NewGame");
    }

    public void ChangeView(string viewName)
    {
        if (_viewMap.TryGetValue(viewName, out Func<UserControl>? createView))
        { MainContent.Content = createView(); }
        else throw new ArgumentException($"Unknown view: {viewName}", nameof(viewName));
    }

    public void LoadCrimeSceneFor(string victimName) =>
        MainContent.Content = new CrimeSceneScreen(this, victimName);

    public void LoadInterviewFor(string interviewee) =>
        MainContent.Content = new InterviewScreen(this, InterviewType.Interview, interviewee, State.LastVictim);

    public void LoadAccusationFor(string interviewee) =>
        MainContent.Content = new InterviewScreen(this, InterviewType.Accusation, interviewee, State.LastVictim);

    public void LoadClueInspectionFor(Clue clue) =>
        MainContent.Content = new ClueScreen(this, clue);

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