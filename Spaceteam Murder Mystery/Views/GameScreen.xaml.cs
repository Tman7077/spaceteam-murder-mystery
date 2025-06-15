namespace SMM.Views;

public partial class GameScreen : UserControl
{
    private readonly MainWindow _main;

    private GameState State { get => _main.State; }

    public GameScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
        header.Content = $"{State.Difficulty} mode selected.";
    }

    private void Kill_Click(object sender, RoutedEventArgs e) =>
        _main.LoadCrimeSceneFor(State.KillCharacter("Courtney"));
        
    private void Talk_Click(object sender, RoutedEventArgs e) =>
        _main.ChangeView("Interviews");
}