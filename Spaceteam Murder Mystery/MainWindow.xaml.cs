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
        if (fullscreen) SourceInitialized += ImmediateFullScreen;

        Activated += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        _viewMap = new()
        {
            { "Title",       () => new TitleScreen(this) },
            { "Settings",    () => new SettingsScreen(this) },
            { "Difficulty",  () => new DifficultyScreen(this) },
            { "NewGame",     () => new GameScreen(this) },
            { "Interviews",  () => new CharacterSelectionScreen(this, "Interviews") },
            { "Accusations", () => new CharacterSelectionScreen(this, "Accusations") },
            { "Voting    ",  () => new CharacterSelectionScreen(this, "Voting") }
        };

        ChangeView("Title");
    }

    public void ChangeView(string viewName)
    {
        if (_viewMap.TryGetValue(viewName, out Func<UserControl>? createView))
        { MainContent.Content = createView(); }
        else MessageBox.Show("View not found: " + viewName, "View Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void StartGame(string? difficulty)
    {
        IDifficulty gameDifficulty = difficulty switch
        {
            "Easy" => new DEasy(),
            "Medium" => new DMedium(),
            "Hard" => new DHard(),
            _ => throw new ArgumentException($"Unknown difficulty: {difficulty}")
        };

        State = new GameState(gameDifficulty);

        ChangeView("NewGame");
    }

    public void LoadCrimeSceneFor(string victimName) =>
        MainContent.Content = new CrimeSceneScreen(this, victimName);

    public void LoadInterviewFor(string interviewee) =>
        MainContent.Content = new InterviewScreen(this, interviewee);

    private void ImmediateFullScreen(object? sender, EventArgs e)
    {
        _windowHandler.ToggleFullScreen();
        SourceInitialized -= ImmediateFullScreen;
    }

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