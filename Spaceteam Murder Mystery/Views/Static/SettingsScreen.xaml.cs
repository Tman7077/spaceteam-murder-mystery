namespace SMM.Views.Static;

public partial class SettingsScreen : UserControl
{
    private readonly MainWindow _main;

    public SettingsScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();

        FullscreenButton.Content = _main.WindowHandler.IsFullScreen ? "Exit" : "Enter";
        MuteButton.Content       = AppSettings.Muted ? "Unmute" : "Mute";
        VolumeSlider.Value       = (int)(AppSettings.Volume * 100);
    }

    public void Fullscreen_Click(object sender, RoutedEventArgs e)
    {
        _main.WindowHandler.ToggleFullScreen();
        FullscreenButton.Content = _main.WindowHandler.IsFullScreen ? "Exit" : "Enter";
    }
    public async void Back_Click(object sender, RoutedEventArgs e) =>
        await _main.ToPreviousScreen();


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
    
    public void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (e.NewValue < 0 || e.NewValue > 100)
        { throw new ArgumentOutOfRangeException(nameof(e), "Volume must be between 0 and 100"); }

        AppSettings.Volume = e.NewValue / 100;
        _main.Soundtrack.ChangeVolume(AppSettings.Volume);
    }
}
