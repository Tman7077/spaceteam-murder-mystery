namespace SMM.Views.Dynamic;

/// <summary>
/// A screen to show a character and their interview/accusation response.
/// </summary>
public partial class InterviewScreen : InspectionScreen
{
    /// <summary>
    /// The type of interview/accusation being made.
    /// </summary>
    private readonly InterviewType _type;

    /// <summary>
    /// The character being interviewed.
    /// </summary>
    private readonly Character _interviewee;

    /// <summary>
    /// The name of the character about whom the character is being interviewed.
    /// </summary>
    private readonly string _victim;

    /// <summary>
    /// Creates a screen to display an interview or accusation response from a character.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    /// <param name="type">The type of interview.</param>
    /// <param name="interviewee">The character being interviewed.</param>
    /// <param name="victim">The short name of the character about whom the interview is.</param>
    public InterviewScreen(MainWindow main, InterviewType type, string interviewee, string victim) : base(main)
    {
        Validator.ValidateCharacter(interviewee, main.State);
        Validator.ValidateCharacter(victim, main.State);

        Character character = main.State.Characters[interviewee];
        _type        = type;
        _interviewee = character;
        _victim      = victim;
        _dir = character.Facing switch
        {
            Direction.Left  => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"Unknown character facing direction: {character.Facing}")
        };

        LoadScreen();
    }

    /// <summary>
    /// Loads the screen with the appropriate content based on the interview type and character.
    /// </summary>
    protected override void LoadScreen()
    {
        Grid root  = LoadScreenSetup();
        Grid block = CreateImageGrid(_interviewee.ProfileUri, _interviewee.Name);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = Difficulties.All[_main.State.Difficulty].GetResponse(_interviewee, _type, _victim),
        };

        Continue_Click continueClick;
        if (_type == InterviewType.Interview)
        { continueClick = async (s, e) => await _main.ToPreviousScreen(); }
        else // if (_type == InterviewType.Accusation)
        {
            continueClick = async (s, e) =>
            {
                _main.State.KillCharacter(new Victim.ByName.Voted(_interviewee.ShortName));
                Vote vote = new(_interviewee.ShortName, _main.State);
                await _main.AdvanceStory(new Advance.PostAccusation(vote));
            };
        }

        LoadScreenFinal(root, block, text, continueClick);
    }
}