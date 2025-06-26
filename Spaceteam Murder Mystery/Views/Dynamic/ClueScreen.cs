namespace SMM.Views.Dynamic;

public partial class ClueScreen : InspectionScreen
{
    private readonly Clue _clue;
    public ClueScreen(MainWindow main, Clue clue) : base(main)
    {
        _clue = clue;
        _dir  = Direction.Left;
        LoadScreen();
    }

    protected override void LoadScreen()
    {
        Grid root = LoadScreenSetup();

        Grid block = CreateImageGrid(_clue.Uri, _clue.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = _clue.Description
        };

        async void continueClick(object sender, RoutedEventArgs e) =>
            await _main.ToPreviousScreen();

        LoadScreenFinal(root, block, text, continueClick);
    }
}
