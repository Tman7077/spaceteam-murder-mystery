namespace SMM.Views.Static;

public partial class GameScreen : UserControl
{
    private readonly MainWindow _main;

    private GameState State { get => _main.State; }

    public GameScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
        Header.Content = $"{State.Difficulty} mode selected.";
    }

    private async void Kill_Click(object sender, RoutedEventArgs e)
    {
        State.KillCharacter(new Victim.ByName.Innocent("Ethan"), out string who);
        await _main.LoadCrimeSceneFor(who);
    }
  
    private async void Talk_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Selection(InterviewType.Interview));
}