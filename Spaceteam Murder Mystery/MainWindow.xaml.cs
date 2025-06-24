namespace SMM;

using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

public partial class MainWindow : Window
{
    private readonly WindowHandler      _windowHandler;
    private readonly ContentControl     _mainControl = new();
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

    public MainWindow()
    {
        InitializeComponent();

        _windowHandler = new(this);

        AppSettings.Load(_windowHandler, out bool fullscreen);
        if (fullscreen)
        { SourceInitialized += ImmediateFullScreen; }

        Activated   += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        Grid mainGrid = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment   = VerticalAlignment.Stretch,
            Children            = { _mainControl }
        };
        Content       = mainGrid;
        CurrentScreen = View.Request(this, new Screen.Title())();
    }

    public async Task StartGame(string difficulty)
    {
        if (!Difficulties.All.ContainsKey(difficulty))
        { throw new ArgumentException($"Unknown difficulty: {difficulty}"); }

        State = new GameState(difficulty);
        await ChangeView(new Screen.Story(FirstLoad: true, null));
    }

    public async Task ChangeView(Screen screen) =>
        await ChangeView(View.Request(this, screen)());
    private async Task ChangeView(UserControl control)
    {
        await FadeAsync(FadeType.Out);
        PrevScreen    = CurrentScreen;
        CurrentScreen = control;
        await FadeAsync(FadeType.In);
    }

    public async Task ToPreviousScreen()
    {
        await FadeAsync(FadeType.Out);
        CurrentScreen = PrevScreen;
        await FadeAsync(FadeType.In);
    }

    public async Task AdvanceStory(Vote vote)
    {
        Screen nextScreen = vote.Success
            ? new Screen.Success(vote)
            : new Screen.Story(FirstLoad: false, vote);
        await ChangeView(nextScreen);
    }
    public async Task AdvanceStory() =>
        await ChangeView(new Screen.Story(FirstLoad: false, null));

    public async Task LoadCrimeSceneFor(string victimName) =>
        await ChangeView(new Screen.CrimeScene(victimName));

    public async Task LoadInterviewFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Interview, interviewee, State.LastVictim));

    public async Task LoadAccusationFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Accusation, interviewee, State.LastVictim));

    public async Task LoadClueInspectionFor(Clue clue) =>
        await ChangeView(new Screen.InspectClue(clue));

    public void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.F11:
                _windowHandler.ToggleFullScreen();
                break;
            case Key.Escape:
                // soon™
                break;
            default:
                break;
        }
    }
    
    private void ImmediateFullScreen(object? sender, EventArgs e) =>
        _windowHandler.ToggleFullScreen();

    private Task<bool> FadeAsync(FadeType inOut, double duration = 0.5)
    {
        Task.Delay(500).Wait();
        var tcs = new TaskCompletionSource<bool>();
        bool fadeIn = inOut == FadeType.In;

        DoubleAnimation animation = new()
        {
            From         = fadeIn ? 0 : 1,
            To           = fadeIn ? 1 : 0,
            Duration     = TimeSpan.FromSeconds(duration),
            FillBehavior = FillBehavior.HoldEnd
        };
        animation.Completed += (_, __) => tcs.SetResult(true);

        _mainControl.BeginAnimation(OpacityProperty, animation);

        return tcs.Task;
    }
    
    protected override void OnClosing(CancelEventArgs e)
    {
        AppSettings.Save(_windowHandler);
        base.OnClosing(e);
    }
}