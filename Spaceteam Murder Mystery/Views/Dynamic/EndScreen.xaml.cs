using System.IO;

namespace SMM.Views.Dynamic;

public partial class EndScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly bool _victory;

    private GameState State { get => _main.State; }

    public EndScreen(MainWindow main, bool victory)
    {
        _main = main;
        InitializeComponent();

        _victory = victory;
        LoadScreen();
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

        Grid root = new()
        { ClipToBounds = true };

        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(9, GridUnitType.Star) });

        Grid.SetRow(viewbox, 0);
        Grid.SetRowSpan(viewbox, 3);
        root.Children.Add(viewbox);

        Image sceneImage = new()
        {
            Source  = AssetHelper.NebulaBG,
            Stretch = Stretch.UniformToFill,
            Width   = canvas.Width,
            Height  = canvas.Height
        };

        Canvas.SetLeft(sceneImage, 0);
        Canvas.SetTop(sceneImage,  0);
        canvas.Children.Add(sceneImage);

        List<string> namesToDisplay = _victory switch
        {
            true  => [.._main.State.Characters.Select(c => c.Key).Where(name => !_main.State.Characters[name].IsGuilty)],
            false => _main.State.Characters.GetGuiltyNames(includeDead: true)
        };
        List<Character> charsToDisplay = [..namesToDisplay.Select(name => _main.State.Characters[name])];
        charsToDisplay.Sort((a, b) => a.EndScreenPos[2].CompareTo(b.EndScreenPos[2]));

        foreach (Character character in charsToDisplay)
        {
            Image portrait = new()
            { Source = new BitmapImage(character.EndProfileUri) };

            MultiplyEffect tint = new();
            if (_victory && !character.IsAlive)
            {
                tint.OverlayColor = Color.FromArgb(192, 0, 0, 0);
                portrait.Effect   = tint;
            }
            else if (!_victory)
            {
                tint.OverlayColor = Color.FromArgb(128, 255, 0, 0);
                portrait.Effect   = tint;
            }

            Canvas.SetLeft(portrait, character.EndScreenPos[0]);
            Canvas.SetTop(portrait,  character.EndScreenPos[1]);
            canvas.Children.Add(portrait);
        }

        Button continueButton = new()
        {
            Style    = (Style)FindResource("CornerCutButton"),
            Content  = "Continue",
            Width    = canvas.Width  * 0.1,
            FontSize = canvas.Height * 0.02,
        };
        continueButton.Click += async (sender, e) =>
            await _main.ChangeView(new Screen.Title());
        Canvas.SetLeft(continueButton, canvas.Width * 0.85);
        Canvas.SetTop(continueButton,  canvas.Height * 0.9);
        canvas.Children.Add(continueButton);

        Label header = new()
        {
            Style      = (Style)FindResource("ScreenHeader"),
            Content    = _victory ? "Victory!" : "Defeat!",
            Foreground = (Brush)FindResource("SMMWhite")
        };
        header.SetBinding(FontSizeProperty, FontScaleExtension.GetBinding(4));
        Grid.SetRow(header, 1);
        root.Children.Add(header);

        Content = root;
    }
}