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

    private async void StartGame_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Difficulty());

    private void ContinueGame_Click(object sender, RoutedEventArgs e) =>
        MessageBox.Show("Coming soon!");

    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings());

    private void Quit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();
}