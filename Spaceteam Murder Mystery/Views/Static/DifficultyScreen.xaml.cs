namespace SMM.Views.Static;

/// <summary>
/// A screen to display difficulty options, that starts a game on click.
/// </summary>
public partial class DifficultyScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// Creates a new difficulty selection screen.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    public DifficultyScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();

        Background = new ImageBrush()
        {
            ImageSource = AssetHelper.NebulaBG,
            Stretch     = Stretch.UniformToFill
        };
    }

    /// <summary>
    /// Handles the click event of a difficulty button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void DifficultyButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        { throw new ArgumentException("Sender is not a Button.", nameof(sender)); }

        string? difficulty = button.Tag?.ToString();
        if (string.IsNullOrEmpty(difficulty))
        { throw new ArgumentException("Button does not have a valid difficulty tag.", button.Name); }

        await _main.StartGame(difficulty);
    }
}