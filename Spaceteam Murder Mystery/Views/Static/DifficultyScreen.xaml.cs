namespace SMM.Views.Static;

public partial class DifficultyScreen : UserControl
{
    private readonly MainWindow _main;

    public DifficultyScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }
    
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