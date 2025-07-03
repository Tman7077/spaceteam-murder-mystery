namespace SMM.Views.Static;

/// <summary>
/// A screen to display application-wide settings.
/// These settings are not pertinent to a run of the game, but
/// rather to the application window itself.
/// </summary>
public partial class SettingsScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// Creates a new screen to display application-wide settings.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    public SettingsScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();

        FullscreenButton.Content = _main.WindowHandler.IsFullScreen ? "Exit" : "Enter";
        MuteButton.Content       = AppSettings.Muted ? "Unmute" : "Mute";
        VolumeSlider.Value       = (int)(AppSettings.Volume * 100);
    }

    /// <summary>
    /// Handles the click event of the Fullscreen button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    public void Fullscreen_Click(object sender, RoutedEventArgs e)
    {
        _main.WindowHandler.ToggleFullScreen();
        FullscreenButton.Content = _main.WindowHandler.IsFullScreen ? "Exit" : "Enter";
    }

    /// <summary>
    /// Handles the click event of the Back button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    public async void Back_Click(object sender, RoutedEventArgs e) =>
        await _main.ToPreviousScreen();

    /// <summary>
    /// Handles the click event of the Mute button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    public async void Mute_Click(object sender, RoutedEventArgs e)
    {
        if (AppSettings.Muted)
        {
            MuteButton.Content = "Mute";
            await _main.Soundtrack.Unmute();
        }
        else
        {
            MuteButton.Content = "Unmute";
            await _main.Soundtrack.Mute();
        }
    }

    /// <summary>
    /// Handles the click event of the Puse on Lose Focus button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    public void PauseOnLoseFocus_Click(object sender, RoutedEventArgs e)
    {
        if (_main.WindowHandler.PauseOnLoseFocus)
        {
            _main.WindowHandler.PauseOnLoseFocus = false;
            PauseOnLoseFocusButton.Content = "Disabled";
        }
        else
        {
            _main.WindowHandler.PauseOnLoseFocus = true;
            PauseOnLoseFocusButton.Content = "Enabled";
        }
    }

    /// <summary>
    /// Handles the value change event of the voluem slider.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    public void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (e.NewValue < 0 || e.NewValue > 100)
        { throw new ArgumentOutOfRangeException(nameof(e), "Volume must be between 0 and 100"); }

        AppSettings.Volume = e.NewValue / 100;
        _main.Soundtrack.ChangeVolume(AppSettings.Volume);
    }
}
