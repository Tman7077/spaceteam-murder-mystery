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
        _main   = main;
        _scene  = new CrimeScene(victim, State);
        _victim = State.Characters[victim];
        LoadScreen();
    }

    public void Clue_Click(object sender, RoutedEventArgs e)
    {
        var something = sender.GetType().GetProperty("Tag")?.GetValue(sender, null) as string;
        MessageBox.Show($"{something} Clicked!");
    }

    private void LoadScreen()
    {
        Canvas canvas = new()
        {
            ClipToBounds = true,
            Width        = 3840,
            Height       = 2160
        };

        Viewbox viewbox = new()
        {
            ClipToBounds        = true,
            Stretch             = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment   = VerticalAlignment.Stretch,
            Child               = canvas
        };

        Border root = new()
        {
            ClipToBounds = true,
            Child        = viewbox
        };

        Image sceneImage = new()
        {
            Source  = new BitmapImage(new Uri(_victim.CrimeSceneImagePath, UriKind.Absolute)),
            Stretch = Stretch.UniformToFill,
            Width   = canvas.Width,
            Height  = canvas.Height,
        };

        Canvas.SetLeft(sceneImage, 0);
        Canvas.SetTop(sceneImage,  0);
        canvas.Children.Add(sceneImage);

        List<Clue> cluesByZ  =  [.. _scene.Clues];
        cluesByZ.Sort((a, b) => a.Z.CompareTo(b.Z));

        foreach (Clue clue in cluesByZ)
        {
            Image clueImage = new()
            { Source = new BitmapImage(new Uri(clue.ImagePath, UriKind.Absolute)) };

            Button imageButton = new()
            {
                Style = (Style)FindResource("ClueImageButton"),
                Content = clueImage,
                Tag = clue.Name
            };
            imageButton.Click += Clue_Click;
            
            Canvas.SetLeft(imageButton, clue.X);
            Canvas.SetTop(imageButton,  clue.Y);
            canvas.Children.Add(imageButton);
        }

        Content = root;
    }
}