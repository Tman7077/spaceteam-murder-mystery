namespace SMM.Views.Dynamic;

public partial class InterviewScreen : InspectionScreen
{
    private readonly InterviewType _type;
    private readonly Character _interviewee;
    private readonly string _victim;
    public InterviewScreen(MainWindow main, InterviewType type, string interviewee, string victim) : base(main)
    {
        Validator.ValidateCharacter(interviewee, main.State);
        Validator.ValidateCharacter(victim,      main.State);
        
        Character character = main.State.Characters[interviewee];
        _type = type;
        _interviewee = character;
        _victim = victim;
        _dir = character.Facing switch 
        {
            Direction.Left  => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"Unknown character facing direction: {character.Facing}")
        };
        
        LoadScreen();
    }

    protected override void LoadScreen()
    {
        Grid root = LoadScreenSetup();

        Grid block = CreateImageGrid(new Uri(_interviewee.ProfileImagePath, UriKind.Absolute), _interviewee.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text = Difficulties.All[_main.State.Difficulty].GetResponse(_interviewee, _type, _victim),
        };

        if (_type == InterviewType.Interview)
        {
            async void continueClick(object sender, RoutedEventArgs e) =>
                await _main.ToPreviousScreen();
            
            LoadScreenFinal(root, block, text, continueClick);
        }
        else // _type == InterviewType.Accusation
        {
            async void continueClick(object sender, RoutedEventArgs e)
            {
                _main.State.KillCharacter(new Victim.ByName.Voted(_interviewee.ShortName));
                Vote vote = new(_interviewee.ShortName, _main.State);
                await _main.AdvanceStory(vote);
            }

            LoadScreenFinal(root, block, text, continueClick);
        }
    }
}