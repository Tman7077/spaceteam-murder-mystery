namespace SMM.Views;

using System.Windows;
using System.Windows.Controls;
using Models;
using Models.Difficulties;
using Services;

public partial class GameScreen : UserControl
{
    private readonly MainWindow _main;
    private string _currentScene;
    private readonly GameState _gameState;
    public GameScreen(MainWindow main, IDifficulty difficulty)
    {
        InitializeComponent();
        _main = main;
        _currentScene = "Intro";
        _gameState = new GameState(difficulty);
        txtStory.Text = _gameState.GetDifficulty() + " mode selected. ";
    }
    private void Choice1_Click(object sender, RoutedEventArgs e)
    {
        txtStory.Text = "In the Engine Room, you find a mysterious scorched glove.";
        // GameState.Find("Scorched Glove");
    }
    private void Choice2_Click(object sender, RoutedEventArgs e)
    {
        txtStory.Text = "In the Crew Quarters, the bed is made—but someone's missing.";
        _currentScene = "Crew Quarters";
    }
}