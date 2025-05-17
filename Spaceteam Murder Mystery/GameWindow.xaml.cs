using System.Windows;

namespace SMM
{
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void Choice1_Click(object sender, RoutedEventArgs e)
        {
            txtStory.Text = "In the Engine Room, you find a mysterious scorched glove.";
            GameState.FoundClues.Add("Scorched Glove");
        }

        private void Choice2_Click(object sender, RoutedEventArgs e)
        {
            txtStory.Text = "In the Crew Quarters, the bed is made—but someone's missing.";
            GameState.CurrentScene = "Crew Quarters";
        }
    }
}