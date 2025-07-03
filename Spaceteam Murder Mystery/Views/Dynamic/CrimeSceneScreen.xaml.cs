namespace SMM.Views.Dynamic;

/// <summary>
/// A screen to display a crime scene for a given victim.
/// </summary>
public partial class CrimeSceneScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// The CrimeScene containing the information to display.
    /// </summary>
    private readonly CrimeScene _scene;

    /// <summary>
    /// The victim of the crime.
    /// </summary>
    private readonly Character _victim;

    /// <summary>
    /// Creates a screen to display a victim's crime scene.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    /// <param name="victim">The short name of the character that is the victim.</param>
    /// <param name="clues">Optionally, a list of clues to guarantee will be displayed.</param>
    public CrimeSceneScreen(MainWindow main, string victim, List<Clue>? clues = null)
    {
        Validator.ValidateCharacter(victim, main.State);

        _main = main;
        InitializeComponent();

        _scene  = new CrimeScene(victim, _main.State);
        _victim = _main.State.Characters[victim];
        LoadScreen(clues);
    }

    /// <summary>
    /// Handles the click event for a clue image button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the object called the method.</param>
    public async void Clue_Click(object sender, RoutedEventArgs e)
    {
        string? itemName = sender.GetType().GetProperty("Tag")?.GetValue(sender) as string;

        if (_scene.Clues.FirstOrDefault(c => c.Name == itemName) is not Clue clue)
        { return; }

        await _main.LoadClueInspectionFor(clue);
    }

    /// <summary>
    /// Loads the crime scene screen with clues.
    /// If no clues are provided, it will load all clues from the scene.
    /// </summary>
    /// <param name="clues">A list of clues to force load.</param>
    private void LoadScreen(List<Clue>? clues = null)
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
            Source  = new BitmapImage(_victim.CrimeSceneUri),
            Stretch = Stretch.UniformToFill,
            Width   = canvas.Width,
            Height  = canvas.Height,
        };

        Canvas.SetLeft(sceneImage, 0);
        Canvas.SetTop(sceneImage,  0);
        canvas.Children.Add(sceneImage);

        clues ??= [.. _scene.Clues];
        clues.Sort((a, b) => a.CrimeScenePos[2].CompareTo(b.CrimeScenePos[2]));

        foreach (Clue clue in clues)
        {
            Image clueImage = new()
            { Source = new BitmapImage(clue.SceneUri) };

            Button imageButton = new()
            {
                Style   = (Style)FindResource("ClueImageButton"),
                Content = clueImage,
                Tag     = clue.Name
            };
            imageButton.Click += Clue_Click;

            Canvas.SetLeft(imageButton, clue.CrimeScenePos[0]);
            Canvas.SetTop(imageButton,  clue.CrimeScenePos[1]);
            canvas.Children.Add(imageButton);
        }

        Button continueButton = new()
        {
            Style = (Style)FindResource("CornerCutButton"),
            Content = "Continue",
            Width = canvas.Width * 0.1,
            FontSize = canvas.Height * 0.02,
        };
        continueButton.Click += async (sender, e) =>
            await _main.ChangeView(new Screen.Selection(InterviewType.Interview));
        Canvas.SetLeft(continueButton, canvas.Width * 0.85);
        Canvas.SetTop(continueButton, canvas.Height * 0.05);
        canvas.Children.Add(continueButton);

        Content = root;
    }
    
    /// <summary>
    /// Gets the names of the owners of all clues in the crime scene,
    /// so they can be saved and subsequently reloaded.
    /// </summary>
    /// <returns>An array of clue owner names.</returns>
    public string[] GetSaveData() => [.. _scene.Clues.Select(c => c.Owner)];
}