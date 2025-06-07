namespace SMM;

using System.ComponentModel;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, Func<UserControl>> _viewMap;
    private readonly WindowHandler _windowHandler;

    public MainWindow()
    {
        InitializeComponent();

        _windowHandler = new(this);
        bool fullscreen = AppSettings.Load(_windowHandler);
        if (fullscreen) SourceInitialized += ImmediateFullScreen;

        Activated   += _windowHandler.MainWindow_Activated;
        Deactivated += _windowHandler.MainWindow_Deactivated;

        _viewMap = new Dictionary<string, Func<UserControl>>
        {
            { "Title",      () => new TitleScreen(this) },
            { "Settings",   () => new SettingsScreen(this) },
            { "Difficulty", () => new DifficultyScreen(this) },
            { "EasyGame",   () => new GameScreen(this, new DEasy()) },
            { "MediumGame", () => new GameScreen(this, new DMedium()) },
            { "HardGame",   () => new GameScreen(this, new DHard()) }
        };
        
        ChangeView("Title");
    }

    public void ChangeView(string viewName)
    {
        if (_viewMap.TryGetValue(viewName, out Func<UserControl>? createView))
        { MainContent.Content = createView(); }
        else
        { MessageBox.Show("View not found: " + viewName, "View Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void LoadCrimeScene(string victimName)
    {
        if (MainContent.Content is GameScreen gameScreen)
        { MainContent.Content = new CrimeSceneScreen(this, gameScreen.State, victimName); }
        else
        { MessageBox.Show("No game screen found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    public void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        { _windowHandler.ToggleFullScreen(); }
    }

    private void ImmediateFullScreen(object? sender, EventArgs e)
    {
        _windowHandler.ToggleFullScreen();
        SourceInitialized -= ImmediateFullScreen;
    }
    protected override void OnClosing(CancelEventArgs e)
    {
        AppSettings.Save(_windowHandler);
        base.OnClosing(e);
    }
}