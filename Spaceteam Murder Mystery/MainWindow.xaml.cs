namespace SMM;

using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

/// <summary>
/// The main window containing all logic and screens for the application.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// The container for each UserControl that displays a "screen" in the game.
    /// </summary>
    private readonly ContentControl _mainControl = new();

    /// <summary>
    /// A stack that keeps track of the history of screens (UserControls) in the game.
    /// </summary>
    private readonly Stack<UserControl> _viewHistory = [];

    /// <summary>
    /// An array of character names in the order they are introduced in the game.
    /// </summary>
    private readonly string[] _orderedCharacterNames =
    [
        "Alex",  "Cade",  "Colter", "Courtney",
        "Ethan", "Jacie", "Olsen",  "Tyler"
    ];

    /// <summary>
    /// The current state of the game, containing information
    /// about the characters, clues, and progress.
    /// </summary>
    private GameState? _gameState;

    /// <summary>
    /// Keeps track of the previous screen in the view history.
    /// When accessed, it pops the last screen from the stack.
    /// </summary>
    private UserControl PrevScreen
    {
        get => _viewHistory.Count > 0
            ?  _viewHistory.Pop()
            :  throw new IndexOutOfRangeException("No previous screen available");
        set => _viewHistory.Push(value);
    }

    /// <summary>
    /// The current screen being displayed in the main content area.
    /// </summary>
    private UserControl CurrentScreen
    {
        get => _mainControl.Content as UserControl
            ?? throw new InvalidOperationException("MainContent is not set to a UserControl");
        set => _mainControl.Content = value;
    }

    /// <summary>
    /// The current state of the game, which is initialized when the game starts.
    /// </summary>
    public GameState State
    {
        get => _gameState
            ?? throw new InvalidOperationException("GameState has not yet been initialized");
        private set => _gameState = value;
    }

    /// <summary>
    /// The soundtrack for the game.
    /// </summary>
    public Soundtrack Soundtrack { get; } = new(SoundtrackType.TitleTheme);

    /// <summary>
    /// Handles window operations regarding size, placement, and fullscreen mode.
    /// </summary>
    public WindowHandler WindowHandler { get; }

    /// <summary>
    /// Creates the main window.
    /// </summary>
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
    }

    /// <summary>
    /// Starts a new game with the specified difficulty level.
    /// </summary>
    /// <param name="difficulty">The difficulty level with which to start the game.</param>
    public async Task StartGame(string difficulty)
    {
        SetupGame(difficulty);
        ChangeMusic(SoundtrackType.MainTheme);
        await ChangeView(new Screen.Story(new Advance.Intro()), 2, 2);
    }

    public void ChangeMusic(SoundtrackType track) =>
        _ = Soundtrack.SwitchTrack(track);

    /// <summary>
    /// Saves the current game state to a file.
    /// </summary>
    public async Task SaveGame()
    {
        UserControl lastView = CurrentScreen;
        while (lastView is not StoryScreen && lastView is not CrimeSceneScreen)
        { lastView = PrevScreen; }
        await GameSaveManager.SaveGameAsync(this, lastView);
    }

    /// <summary>
    /// Loads a saved game from a file.
    /// If no saved game is found, it prompts the user to start a new game.
    /// </summary>
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

    /// <summary>
    /// Sets the game state to the specified state.
    /// This is used to update the game state after loading a saved game.
    /// </summary>
    /// <param name="state">The Gamestate to which to overwrite the current state.</param>
    public void SetGameState(GameState state) => State = state;

    /// <summary>
    /// Changes the current screen to the specified screen with optional fade-out and fade-in effects.
    /// </summary>
    /// <param name="screen">A Screen record for easy initialization.</param>
    /// <param name="fadeOutSeconds">The time over which the current screen should fade out.</param>
    /// <param name="fadeInSeconds">The time over which the new screen should fade in.</param>
    public async Task ChangeView(Screen screen, double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5) =>
        await ChangeView(View.Request(this, screen)(), fadeOutSeconds, fadeInSeconds);
    
    /// <summary>
    /// Changes the current screen to the specified UserControl with optional fade-out and fade-in effects.
    /// This version of the method allows for more flexibility in specifying the UserControl directly.
    /// </summary>
    /// <param name="control">A UserControl to set as the current screen.</param>
    /// <param name="fadeOutSeconds">The time over which the current screen should fade out.</param>
    /// <param name="fadeInSeconds">The time over which the new screen should fade in.</param>
    private async Task ChangeView(UserControl control, double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5)
    {
        await FadeAsync(FadeType.Out, fadeOutSeconds);
        PrevScreen    = CurrentScreen;
        CurrentScreen = control;
        await FadeAsync(FadeType.In, fadeInSeconds);
    }

    /// <summary>
    /// Transitions to the previous screen in the view history with fade-out and fade-in effects.
    /// </summary>
    /// <param name="fadeOutSeconds">The time over which the current screen should fade out.</param>
    /// <param name="fadeInSeconds">The time over which the new screen should fade in.</param>
    public async Task ToPreviousScreen(double fadeOutSeconds = 0.5, double fadeInSeconds = 0.5)
    {
        await FadeAsync(FadeType.Out, fadeOutSeconds);
        CurrentScreen = PrevScreen;
        await FadeAsync(FadeType.In, fadeInSeconds);
    }

    /// <summary>
    /// Changes the current screen to a StoryScreen with the specified Advance.
    /// </summary>
    /// <param name="advance">The type of story Advance.</param>
    public async Task AdvanceStory(Advance advance) =>
        await ChangeView(new Screen.Story(advance));

    /// <summary>
    /// Loads the crime scene for the specified victim.
    /// </summary>
    /// <param name="victimName">The short name of the victim whose crime scene to load.</param>
    public async Task LoadCrimeSceneFor(string victimName) =>
        await ChangeView(new Screen.CrimeScene(victimName));

    /// <summary>
    /// Loads the interview screen for the specified interviewee and the most recent victim.
    /// </summary>
    /// <param name="interviewee">The short name of the character whose interview to load.</param>
    public async Task LoadInterviewFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Interview, interviewee, State.LastVictim));

    /// <summary>
    /// Loads the accusation screen for the specified interviewee and the most recent victim.
    /// </summary>
    /// <param name="interviewee">The short name of the character whose accusation to load.</param>
    public async Task LoadAccusationFor(string interviewee) =>
        await ChangeView(new Screen.InspectChar(InterviewType.Accusation, interviewee, State.LastVictim));

    /// <summary>
    /// Loads the clue inspection screen for the specified clue.
    /// </summary>
    /// <param name="clue">The clue whose inspection to load.</param>
    public async Task LoadClueInspectionFor(Clue clue) =>
        await ChangeView(new Screen.InspectClue(clue));

    /// <summary>
    /// Loads the character introduction screen for the specified character number.
    /// The number will map to the character's name in the predefined order they are introduced.
    /// </summary>
    /// <param name="i">The index of the character to load.</param>
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

    /// <summary>
    /// Handles keypresses for the main window.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
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

    /// <summary>
    /// Initializes the game state based on the selected difficulty.
    /// </summary>
    /// <param name="difficulty">The difficulty level with which to initialize the GameState.</param>
    private void SetupGame(string difficulty)
    {
        if (!Difficulties.All.ContainsKey(difficulty))
        { throw new ArgumentException($"Unknown difficulty: {difficulty}"); }

        State = new GameState(difficulty);
    }

    /// <summary>
    /// Based on the current screen, this method will
    /// pause the game, return to the previous screen,
    /// or do nothing if the current screen is the TitleScreen.
    /// </summary>
    private async Task MaybePause()
    {
        if (CurrentScreen is TitleScreen) return;

        if (CurrentScreen is PauseScreen or DifficultyScreen or SettingsScreen)
        { await ToPreviousScreen(0, 0); }
        else
        { await ChangeView(new Screen.Pause(), 0, 0); }
    }

    /// <summary>
    /// Immediately toggles the fullscreen mode of the window.
    /// This method is called when the window is initialized with fullscreen mode enabled.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private void ImmediateFullScreen(object? sender, EventArgs e) =>
        WindowHandler.ToggleFullScreen();

    /// <summary>
    /// Fades the main control in or out over a specified duration.
    /// </summary>
    /// <param name="inOut">The direction of the fade.</param>
    /// <param name="duration">The length of time over which to fade.</param>
    /// <returns>True when completed.</returns>
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

    /// <summary>
    /// Fades the main control in or out over a specified number of seconds.
    /// This is a convenience method that calls the FadeAsync method with a TimeSpan.
    /// </summary>
    /// <param name="inOut">The direction of the fade.</param>
    /// <param name="seconds">The length of time over which to fade.</param>
    /// <returns>True when completed.</returns>
    private Task<bool> FadeAsync(FadeType inOut, double seconds = 0.5) =>
        FadeAsync(inOut, TimeSpan.FromSeconds(seconds));

    /// <summary>
    /// Adds additional logic to handle the closing of the main window.
    /// This method prompts the user to save their progress before quitting,
    /// and saves the application settings.
    /// </summary>
    /// <param name="e">The arguments with which the method was called.</param>
    protected async override void OnClosing(CancelEventArgs e)
    {
        bool askSave = true;
        if (CurrentScreen is TitleScreen      ||
            CurrentScreen is DifficultyScreen ||
            (CurrentScreen is SettingsScreen  &&
            _viewHistory.Peek() is TitleScreen))
        { askSave = false; }

        if (askSave)
        {
            MessageBoxResult result = MessageBox.Show(
                "Would you like to save your progress?",
                "Quit",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            switch (result)
            {
                case MessageBoxResult.Yes:
                    await SaveGame();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                case MessageBoxResult.None:
                    return;
            }
        }

        AppSettings.Save(WindowHandler);
        base.OnClosing(e);
    }
}