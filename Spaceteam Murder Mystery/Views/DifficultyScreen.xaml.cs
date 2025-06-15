namespace SMM.Views;

public partial class DifficultyScreen : UserControl
{
    private readonly MainWindow _main;

    public DifficultyScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }
    private void DifficultyButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            MessageBox.Show("Sender was not a valid button.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        string? difficulty = button.Tag?.ToString();
        if (string.IsNullOrEmpty(difficulty))
        { throw new ArgumentException("Button does not have a valid difficulty tag.", button.Name); }
        
        _main.StartGame(difficulty);
    }
}