namespace SMM.Views.Static;

/// <summary>
/// A screen to display pause options.
/// </summary>
public partial class PauseScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// Creates a new pause screen to display pause options.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    public PauseScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event of the Back button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void Back_Click(object sender, RoutedEventArgs e) =>
        await _main.ToPreviousScreen(0, 0);

    /// <summary>
    /// Handles the click event of the Settings button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings(), 0, 0);

    /// <summary>
    /// Handles the click event of the Quit to Title button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void QuitToTitle_Click(object sender, RoutedEventArgs e) =>
        await Quit(async () => await _main.ChangeView(new Screen.Title()));

    /// <summary>
    /// Handles the click event of the Quit to Desktop button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private void QuitToDesktop_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

    /// <summary>
    /// Confirms that the user actually wants to quit, and offers the option to save the game if so.
    /// </summary>
    /// <param name="quitHere">Where to quit (title or desktop).</param>
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