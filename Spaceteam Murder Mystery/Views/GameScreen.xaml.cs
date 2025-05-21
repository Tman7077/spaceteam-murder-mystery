using System.Windows;
using System.Windows.Controls;
using SMM.Services;

namespace SMM.Views
{
    public partial class GameScreen : UserControl
    {
        private MainWindow _main;
        private static string CurrentScene = "Intro";
        public GameScreen(MainWindow main)
        {
            InitializeComponent();
            _main = main;
        }

        private void Choice1_Click(object sender, RoutedEventArgs e)
        {
            txtStory.Text = "In the Engine Room, you find a mysterious scorched glove.";
            GameState.Find("Scorched Glove");
        }

        private void Choice2_Click(object sender, RoutedEventArgs e)
        {
            txtStory.Text = "In the Crew Quarters, the bed is made—but someone's missing.";
            CurrentScene = "Crew Quarters";
        }
    }
}