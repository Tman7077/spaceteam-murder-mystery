namespace SMM.Views;

public partial class TitleScreen : UserControl
{
    // Reference to the main window
    private readonly MainWindow _main;

    public TitleScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }
    private void StartGame_Click(object sender, RoutedEventArgs e)
    {
        // Navigate to the game window
        _main.ChangeView("Difficulty");
    }
    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        _main.ChangeView("Settings");
        return;
    }
    private void Quit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}