namespace SMM.Views;

public partial class GameScreen : UserControl
{
    private readonly MainWindow _main;
    // private string _currentScene;
    private readonly GameState _gameState;
    public GameState State { get => _gameState; }
    public GameScreen(MainWindow main, IDifficulty difficulty)
    {
        InitializeComponent();
        _main = main;
        // _currentScene = "Intro";
        _gameState = new GameState(difficulty);
        header.Content = _gameState.GetDifficultyName() + " mode selected. ";
    }
    private void Kill_Click(object sender, RoutedEventArgs e)
    {
        _main.LoadCrimeScene(_gameState.KillCharacter());
    }
}