namespace SMM.Views.Static;

public partial class TitleScreen : UserControl
{
    private readonly MainWindow _main;

    public TitleScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
        Background = new ImageBrush()
        {
            ImageSource = AssetHelper.NebulaBG,
            Stretch     = Stretch.UniformToFill
        };
    }

    private async void StartGame_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Difficulty());

    private async void ContinueGame_Click(object sender, RoutedEventArgs e) =>
        await _main.LoadGame();

    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings(), 0, 0);

    private void Quit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();
}