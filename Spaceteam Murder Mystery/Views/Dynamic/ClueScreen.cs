namespace SMM.Views.Dynamic;

/// <summary>
/// A screen to display a clue and its description.
/// </summary>
public partial class ClueScreen : InspectionScreen
{
    /// <summary>
    /// The clue to display.
    /// </summary>
    private readonly Clue _clue;
    
    /// <summary>
    /// Creates a new ClueScreen for the provided clue.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    /// <param name="clue">The clue to display.</param>
    public ClueScreen(MainWindow main, Clue clue) : base(main)
    {
        _clue = clue;
        _dir  = Direction.Left;
        LoadScreen();
    }

    /// <summary>
    /// Fills the screen with the clue's image and description.
    /// </summary>
    protected override void LoadScreen()
    {
        Grid root  = LoadScreenSetup();
        Grid block = CreateImageGrid(_clue.CleanUri, _clue.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = _clue.Description
        };

        async Task continueClick(object sender, RoutedEventArgs e) =>
            await _main.ToPreviousScreen();

        LoadScreenFinal(root, block, text, continueClick);
    }
}
