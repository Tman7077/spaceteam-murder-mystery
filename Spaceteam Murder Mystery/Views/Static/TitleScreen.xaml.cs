namespace SMM.Views.Static;

public partial class TitleScreen : UserControl
{
    // Reference to the main window
    private readonly MainWindow _main;

    public TitleScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }

    private void StartGame_Click(object sender, RoutedEventArgs e) =>
        _main.ChangeView("Difficulty");

    private void ContinueGame_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Coming soon!");

    private void Settings_Click(object sender, RoutedEventArgs e) =>
        _main.ChangeView("Settings");

    private void Quit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();
}