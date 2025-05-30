namespace SMM.Views;

using System.Windows;
using System.Windows.Controls;

public partial class DifficultyScreen : UserControl
{
    // Reference to the main window
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
        string? difficulty = button.Tag.ToString();
        switch (difficulty)
        {
            case "Easy":
            case "Medium":
            case "Hard":
                break;
            default:
                MessageBox.Show($"Unknown difficulty \"{difficulty}\" selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
        }
        // Navigate to the game window
        _main.ChangeView($"{difficulty}Game");
    }
}