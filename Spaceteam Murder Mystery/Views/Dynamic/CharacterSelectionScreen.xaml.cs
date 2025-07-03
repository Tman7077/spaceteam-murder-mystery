namespace SMM.Views.Dynamic;

/// <summary>
/// Displays a grid of all of the characters with buttons beneath each one.
/// The buttons allow the user to select a character to interview or accuse.
/// </summary>
public partial class CharacterSelectionScreen : UserControl
{
    /// <summary>
    /// The main window of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// The type of interview or accusation being made.
    /// </summary>
    private readonly InterviewType _type;

    /// <summary>
    /// The content of the label on the buttons,
    /// which is either "Interview" or "Accuse"
    /// </summary>
    private readonly string _labelContent;

    /// <summary>
    /// Creates a new character selection screen for the given type of interview or accusation.
    /// </summary>
    /// <param name="main">The application's MainWindow.</param>
    /// <param name="type">The type of interview or accusation being made.</param>
    public CharacterSelectionScreen(MainWindow main, InterviewType type)
    {
        _main = main;
        InitializeComponent();

        _type = type;
        _labelContent = _type switch
        {
            InterviewType.Interview  => "Interview",
            InterviewType.Accusation => "Accuse",
            _ => throw new ArgumentException($"Unknown type: {type}")
        };

        LoadScreen();
    }

    /// <summary>
    /// Handles the click event for the interview button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the object called the method.</param>
    public async void Interview_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string characterName)
        { await _main.LoadInterviewFor(characterName); }
    }

    /// <summary>
    /// Handles the click event for the accuse button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the object called the method.</param>
    public async void Accuse_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string characterName)
        { await _main.LoadAccusationFor(characterName); }
    }

    /// <summary>
    /// Fills the screen with a grid of character blocks.
    /// </summary>
    private void LoadScreen()
    {
        Grid mainGrid = new();

        foreach (int h in (int[])[1, 2, 5, 5, 1])
        { mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(h, GridUnitType.Star) }); }

        foreach (int w in (int[])[1, 3, 3, 3, 3, 1])
        { mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(w, GridUnitType.Star) }); }

        int i = 0;
        foreach (Character character in _main.State.Characters.Values)
        {
            Grid charGrid = CreateCharacterBlock(character);
            int column = i % 4 + 1;
            int row    = i / 4 + 2;
            i++;
            Grid.SetColumn(charGrid, column);
            Grid.SetRow(charGrid,    row);
            mainGrid.Children.Add(charGrid);
        }

        Button continueButton = new()
        { Style = (Style)FindResource("CornerCutButton") };

        if (_type == InterviewType.Interview)
        {
            continueButton.Content = "Accuse";
            continueButton.Click  += async (sender, e) =>
                await _main.ChangeView(new Screen.Selection(InterviewType.Accusation));
        }
        else // if (_type == InterviewType.Accusation)
        {
            continueButton.Content = "Skip";
            continueButton.Click  += async (sender, e) =>
                await _main.AdvanceStory(new Advance.PostAccusation(new Vote.None()));
        }

        Button backButton = new()
        {
            Style   = (Style)FindResource("CornerCutButton"),
            Content = "Back",
        };
        backButton.Click += async (sender, e) =>
            await _main.ToPreviousScreen();

        backButton.SetBinding(HeightProperty,     AspectRatioExtension.GetBinding(0.2));
        backButton.SetBinding(MarginProperty,     GridColumnMarginExtension.GetBinding(0.1));
        continueButton.SetBinding(HeightProperty, AspectRatioExtension.GetBinding(0.2));
        continueButton.SetBinding(MarginProperty, GridColumnMarginExtension.GetBinding(0.1));

        Grid.SetRow(backButton,        1);
        Grid.SetRow(continueButton,    1);
        Grid.SetColumn(backButton,     1);
        Grid.SetColumn(continueButton, 4);

        mainGrid.Children.Add(backButton);
        mainGrid.Children.Add(continueButton);

        Content = mainGrid;
    }

    /// <summary>
    /// Creates a grid block for a character,
    /// which contains their portrait, name, and a button to select them.
    /// </summary>
    /// <param name="character">The character to load.</param>
    /// <returns>A complete Grid containing the character's information.</returns>
    private Grid CreateCharacterBlock(Character character)
    {
        Grid charGrid = new();

        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        Image portrait = new()
        { Source = new BitmapImage(character.ProfileUri) };

        Label nameLabel = new()
        {
            Style      = (Style)FindResource("BodyTextLabel"),
            Content    = character.Name,
            Foreground = (Brush)FindResource("SMMWhite")
        };

        Button selectButton = new()
        {
            Style   = (Style)FindResource("CornerCutButton"),
            Content = _labelContent,
            Tag     = character.ShortName
        };

        if (!character.IsAlive)
        {
            selectButton.IsEnabled  = false;
            selectButton.Opacity    = 0.5;
            selectButton.Background = (Brush)FindResource("SMMBlueGray");

            MultiplyEffect tint = new()
            { OverlayColor = Color.FromArgb(128, 0, 0, 0) };
            portrait.Effect = tint;
        }

        selectButton.Click +=
            _type == InterviewType.Interview
            ? Interview_Click
            : Accuse_Click;

        selectButton.SetBinding(HeightProperty, AspectRatioExtension.GetBinding(0.2));
        selectButton.SetBinding(MarginProperty, ParentBasedMarginExtension.GetBinding(0.1));

        Grid.SetRow(portrait,     0);
        Grid.SetRow(nameLabel,    1);
        Grid.SetRow(selectButton, 2);

        charGrid.Children.Add(portrait);
        charGrid.Children.Add(nameLabel);
        charGrid.Children.Add(selectButton);

        return charGrid;
    }
}
