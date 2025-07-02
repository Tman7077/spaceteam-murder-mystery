namespace SMM;

using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

public partial class MainWindow : Window
{
    private readonly ContentControl     _mainControl = new();
    private readonly Stack<UserControl> _viewHistory = [];
    private readonly string[]           _orderedCharacterNames =
    [
        "Alex", "Cade", "Colter", "Courtney",
        "Ethan", "Jacie", "Olsen", "Tyler"
    ];
    private GameState? _gameState;

    private UserControl PrevScreen
    {
        get => _viewHistory.Count > 0
            ?  _viewHistory.Pop()
            :  throw new IndexOutOfRangeException("No previous screen available");
        set => _viewHistory.Push(value);
    }
    private UserControl CurrentScreen
    {
        get => _mainControl.Content as UserControl
            ?? throw new InvalidOperationException("MainContent is not set to a UserControl");
        set => _mainControl.Content = value;
    }
    public GameState State
    {
        get => _gameState
            ?? throw new InvalidOperationException("GameState has not yet been initialized");
        private set => _gameState = value;
    }
    public Soundtrack    Soundtrack     { get; } = new(SoundtrackType.TitleTheme);
    public WindowHandler WindowHandler  { get; }

    public MainWindow()
    {
        InitializeComponent();

        WindowHandler = new(this);

        AppSettings.Load(WindowHandler, out bool fullscreen);
        if (fullscreen)
        { SourceInitialized += ImmediateFullScreen; }

        Activated   += WindowHandler.MainWindow_Activated;
        Deactivated += WindowHandler.MainWindow_Deactivated;

        Grid mainGrid = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment   = VerticalAlignment.Stretch,
            Children            = { _mainControl }
        };
        Content       = mainGrid;
        CurrentScreen = View.Request(this, new Screen.Title())();

        _ = Soundtrack.Start();
    }

    private void SetupGame(string difficulty)
    {
        if (!Difficulties.All.ContainsKey(difficulty))
        { throw new ArgumentException($"Unknown difficulty: {difficulty}"); }

        State = new GameState(difficulty);
    }
    
    public async Task StartGame(string difficulty)
    {
        SetupGame(difficulty);
        _ = Soundtrack.SwitchTrack(SoundtrackType.MainTheme);
        await ChangeView(new Screen.Story(new Advance.Intro()), 2, 2);
    }

    public async Task SaveGame()
    {
        UserControl lastView = CurrentScreen;
        while (lastView is not StoryScreen && lastView is not CrimeSceneScreen)
        { lastView = PrevScreen; }
        await GameSaveManager.SaveGameAsync(this, lastView);
    }
    
    public async Task LoadGame()
    {
        UserControl? lastView = await GameSaveManager.LoadGameAsync(this);
        if (lastView is null)
        {
            MessageBox.Show("No saved game found. Please start a new game.");
            return;
        }
        _viewHistory.Clear();
        await ChangeView(lastView).ContinueWith(_ => _ = Soundtrack.SwitchTrack(SoundtrackType.MainTheme));
    }
    
    public void SetGameState(GameState state) => State = state;

    public async Task ChangeView(Screen screen, double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5) =>
        await ChangeView(View.Request(this, screen)(), fadeOutSeconds, fadeInSeconds);
    private async Task ChangeView(UserControl control, double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5)
    {
        await FadeAsync(FadeType.Out, fadeOutSeconds);
        PrevScreen = CurrentScreen;
        CurrentScreen = control;
        await FadeAsync(FadeType.In, fadeInSeconds);
    }

    public async Task ToPreviousScreen(double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5)
    {
        await FadeAsync(FadeType.Out, fadeOutSeconds);
        CurrentScreen = PrevScreen;
        await FadeAsync(FadeType.In, fadeInSeconds);
    }
    
    public async Task AdvanceStory(Advance advance) =>
        await ChangeView(new Screen.Story(advance));

    public async Task LoadCrimeSceneFor(string victimName) =>
        await ChangeView(new Screen.CrimeScene(victimName));

    public async Task LoadInterviewFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Interview, interviewee, State.LastVictim));

    public async Task LoadAccusationFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Accusation, interviewee, State.LastVictim));

    public async Task LoadClueInspectionFor(Clue clue) =>
        await ChangeView(new Screen.InspectClue(clue));

    public async Task LoadCharacterIntroNumber(int i)
    {
        if (i == 0)
        {
            string[] characterNames = [.. State.Characters.Keys];
            foreach (string name in _orderedCharacterNames)
            {
                if (!characterNames.Contains(name))
                    throw new ArgumentException($"Character {name} not found in game state");
            }
        }

        string character = _orderedCharacterNames[i];
        Validator.ValidateCharacter(character, State);
        await ChangeView(new Screen.CharacterIntro(character, i));
    }

    public async void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F11:
                WindowHandler.ToggleFullScreen();
                break;
            case Key.Escape:
                await MaybePause();
                break;
        }
    }

    private async Task MaybePause()
    {
        if (CurrentScreen is TitleScreen) return;

        if (CurrentScreen is PauseScreen or DifficultyScreen or SettingsScreen)
        { await ToPreviousScreen(0, 0); }
        else
        { await ChangeView(new Screen.Pause(), 0, 0); }
    }
    private void ImmediateFullScreen(object? sender, EventArgs e) =>
        WindowHandler.ToggleFullScreen();

    private Task<bool> FadeAsync(FadeType inOut, TimeSpan duration)
    {
        if (duration.TotalMilliseconds < 0)
        { throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than or equal to zero"); }

        if (duration.TotalMilliseconds == 0)
        { return Task.FromResult(true); }

        TaskCompletionSource<bool> tcs = new();
        bool fadeIn = inOut == FadeType.In;

        DoubleAnimation animation = new()
        {
            From         = fadeIn ? 0 : 1,
            To           = fadeIn ? 1 : 0,
            Duration     = duration,
            FillBehavior = FillBehavior.HoldEnd
        };
        animation.Completed += (_, __) => tcs.SetResult(true);

        _mainControl.BeginAnimation(OpacityProperty, animation);

        return tcs.Task;
    }
    private Task<bool> FadeAsync(FadeType inOut, double seconds = 0.5) =>
        FadeAsync(inOut, TimeSpan.FromSeconds(seconds));
    
    protected override void OnClosing(CancelEventArgs e)
    {
        AppSettings.Save(WindowHandler);
        base.OnClosing(e);
    }
}