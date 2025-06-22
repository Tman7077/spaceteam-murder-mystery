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

        Grid block = CreateImageGrid(new Uri(_clue.ImagePath, UriKind.Absolute), _clue.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = _clue.Description
        };

        void continueClick(object sender, RoutedEventArgs e) =>
            _main.ToPreviousScreen();

        LoadScreenFinal(root, block, text, continueClick);
    }
}
