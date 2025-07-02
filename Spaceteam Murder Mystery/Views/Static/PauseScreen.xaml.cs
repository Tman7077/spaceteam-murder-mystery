namespace SMM.Views.Static;

public partial class PauseScreen : UserControl
{
    private readonly MainWindow _main;

    public PauseScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }

    private async void Back_Click(object sender, RoutedEventArgs e) =>
        await _main.ToPreviousScreen(0, 0);

    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings(), 0, 0);

    private async void QuitToTitle_Click(object sender, RoutedEventArgs e) =>
        await Quit(async () => await _main.ChangeView(new Screen.Title()));

    private  void QuitToDesktop_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();
    
    private async Task Quit(Func<Task> quitHere)
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
                await _main.SaveGame();
                await quitHere();
                break;
            case MessageBoxResult.No:
                await quitHere();
                break;
            case MessageBoxResult.Cancel:
            case MessageBoxResult.None:
                break;
        }
    }
}