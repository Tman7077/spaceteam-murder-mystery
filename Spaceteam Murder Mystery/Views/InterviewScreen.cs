namespace SMM.Views;

public partial class InterviewScreen : InspectionScreen
{
    private readonly InterviewType _type;
    private readonly Character _interviewee;
    private readonly string _victim;
    public InterviewScreen(MainWindow main, InterviewType type, string interviewee, string victim) : base(main)
    {
        if (main.State.Characters[interviewee] is not Character character)
        { throw new ArgumentException($"Character '{interviewee}' not found in game data."); }

        _type = type;
        _interviewee = character;
        _victim = victim;
        _dir = !character.Facing;
        LoadScreen();
    }

    protected override void LoadScreen()
    {
        (Grid root, bool left) = LoadScreenSetup();

        Grid block = CreateImageGrid(new Uri(_interviewee.ProfileImagePath, UriKind.Absolute), _interviewee.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = Difficulties.All[_main.State.Difficulty].GetResponse(_interviewee, _type, _victim),
        };

        if (left)
        {
            Grid.SetColumn(block, 1);
            Grid.SetColumn(text,  3);
        }
        else
        {
            Grid.SetColumn(block, 3);
            Grid.SetColumn(text,  1);
        }

        LoadScreenFinal(root, block, text);
    }
}