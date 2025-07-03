namespace SMM.Views.Dynamic;

/// <summary>
/// Screen to show a character's introduction.
/// </summary>
public partial class CharacterIntroScreen : InspectionScreen
{
    /// <summary>
    /// The character whose introduction to show.
    /// </summary>
    private readonly Character _character;

    /// <summary>
    /// The number in a predefined order in which this character is introduced.
    /// </summary>
    private readonly int _index;

    /// <summary>
    /// Creates a new character introduction screen for a character,
    /// given the index in the introduction order.
    /// </summary>
    /// <param name="main">The application's MainWindow.</param>
    /// <param name="name">The name of the character to display.</param>
    /// <param name="i">The character's index in the introduction order.</param>
    public CharacterIntroScreen(MainWindow main, string name, int i) : base(main)
    {
        Character character = main.State.Characters[name];
        _character = character;
        _index     = i;
        _dir = character.Facing switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"Unknown character facing direction: {character.Facing}")
        };

        LoadScreen();
    }

    /// <summary>
    /// Loads the screen with the character's introduction.
    /// </summary>
    protected override void LoadScreen()
    {
        Grid root = LoadScreenSetup();

        TextBlock mottoBlock = new()
        {
            Style               = (Style)FindResource("BodyTextBlock"),
            Foreground          = (Brush)FindResource("SMMOrange"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center,
            TextWrapping        = TextWrapping.Wrap,
            Text                = _character.Motto
        };
        mottoBlock.SetBinding(FontSizeProperty, FontScaleExtension.GetBinding(0.75));

        Grid block = CreateImageGrid(_character.ProfileUri, _character.Name, mottoBlock);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = _character.Description,
        };

        Continue_Click continueClick;
        if (_index + 1 == _main.State.Characters.Count)
            continueClick = async (s, e) => await _main.AdvanceStory(new Advance.FirstMurder());
        else
            continueClick = async (s, e) => await _main.LoadCharacterIntroNumber(_index + 1);

        LoadScreenFinal(root, block, text, continueClick);

    }
}