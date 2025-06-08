namespace SMM.Views;

public partial class CrimeSceneScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly CrimeScene _scene;
    private readonly Character _victim;
    private GameState State { get => _main.State; }
    public CrimeSceneScreen(MainWindow main, string victim)
    {
        InitializeComponent();
        _main = main;
        _scene = new CrimeScene(victim, State);
        _victim = State.Characters[victim];
        LoadScreen();
    }

    public void Clue_Click(object sender, RoutedEventArgs e)
    {
        var something = sender.GetType();
        MessageBox.Show($"{something} Clicked!");
    }

    private void LoadScreen()
    {
        SP.Children.Clear();

        Label label1 = new()
        {
            Content = $"Victim: {_victim.Name}",
            Style = (Style)FindResource("ScreenHeader")
        };

        Label label2 = new()
        {
            Content = $"Difficulty: {State.GetDifficultyName()}",
            Style = (Style)FindResource("BodyText")
        };

        SP.Children.Add(label1);
        SP.Children.Add(label2);

        Image sceneImage = new()
        {
            Source = new BitmapImage(new Uri(_victim.CrimeSceneImagePath, UriKind.Absolute)),
            Width = 320,
            Height = 180,
            Margin = new Thickness(10)
        };
        SP.Children.Add(sceneImage);

        StackPanel cluesPanel = new()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(10)
        };

        foreach (Clue clue in _scene.Clues)
        {
            Image clueImage = new()
            {
                Source = new BitmapImage(new Uri(clue.ImagePath, UriKind.Absolute)),
                Width = 30,
                Height = 30,
                Margin = new Thickness(5)
            };
            Button imageButton = new()
            {
                Style = (Style)FindResource("ClueImageButton"),
                Content = clueImage,
            };
            imageButton.Click += Clue_Click;
            cluesPanel.Children.Add(imageButton);
        }
        SP.Children.Add(cluesPanel);
    }
}