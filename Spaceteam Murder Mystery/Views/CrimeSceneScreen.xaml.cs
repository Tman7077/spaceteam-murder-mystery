namespace SMM.Views;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Models;
using Services;

public partial class CrimeSceneScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly CrimeScene _scene;
    private readonly Character _victim;
    public CrimeSceneScreen(MainWindow main, GameState state, string victim)
    {
        InitializeComponent();
        _main = main;
        _scene = new CrimeScene(victim, state);
        _victim = _scene.State.Characters[victim];
        LoadScreen();
    }
    private void LoadScreen()
    {
        // HashSet<Clue> clues = [];
        SP.Children.Clear();

        Label label1 = new()
        {
            Content = $"Victim: {_victim.Name}",
            Style = (Style)FindResource("ScreenHeader")
        };

        Label label2 = new()
        {
            Content = $"Difficulty: {_scene.State.GetDifficulty()}",
            Style = (Style)FindResource("BodyText")
        };

        SP.Children.Add(label1);
        SP.Children.Add(label2);

        Image image = new()
        {
            Source = new BitmapImage(new Uri(_victim.CrimeSceneImagePath, UriKind.Absolute)),
            Width = 320,
            Height = 180,
            Margin = new Thickness(10)
        };
        SP.Children.Add(image);

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
            cluesPanel.Children.Add(clueImage);
        }
        SP.Children.Add(cluesPanel);
    }
}