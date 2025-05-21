using System.Windows;
using System.Windows.Controls;

namespace SMM.Views
{
    public partial class TitleScreen : UserControl
    {
        // Reference to the main window
        private MainWindow _main;

        public TitleScreen(MainWindow main)
        {
            InitializeComponent();
            _main = main;
        }
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the game window
            _main.ChangeView("Game");
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}