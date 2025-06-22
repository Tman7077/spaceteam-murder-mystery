namespace SMM.Views.Dynamic;

public partial class CrimeSceneScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly CrimeScene _scene;
    private readonly Character _victim;

    private GameState State { get => _main.State; }
    
    public CrimeSceneScreen(MainWindow main, string victim)
    {
        Validator.ValidateCharacter(victim, main.State);

        InitializeComponent();
        _main = main;
        _scene = new CrimeScene(victim, State);
        _victim = State.Characters[victim];
        LoadScreen();
    }

    public void Clue_Click(object sender, RoutedEventArgs e)
    {
        string? itemName = sender.GetType().GetProperty("Tag")?.GetValue(sender) as string;

        if (_scene.Clues.FirstOrDefault(c => c.Name == itemName) is not Clue clue)
        { return; }

        _main.LoadClueInspectionFor(clue);
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
                Style   = (Style)FindResource("ClueImageButton"),
                Content = clueImage,
                Tag     = clue.Name
            };
            imageButton.Click += Clue_Click;
            
            Canvas.SetLeft(imageButton, clue.X);
            Canvas.SetTop(imageButton,  clue.Y);
            canvas.Children.Add(imageButton);
        }

        Button continueButton = new()
        {
            Style    = (Style)FindResource("CornerCutButton"),
            Content  = "Continue",
            Width    = canvas.Width  * 0.1,
            FontSize = canvas.Height * 0.02,
        };
        continueButton.Click += (sender, e) =>
            _main.ChangeView(new Screen.Selection(InterviewType.Interview));
        Canvas.SetLeft(continueButton, canvas.Width * 0.85);
        Canvas.SetTop(continueButton,  canvas.Height * 0.05);
        canvas.Children.Add(continueButton);

        Content = root;
    }
}