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
        
        _main.StartGame(button.Tag.ToString());
    }
}