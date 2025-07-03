namespace SMM.Views.Static;

/// <summary>
/// A screen to display various parts of the story.
/// </summary>
public partial class StoryScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// The type of story advance, used to determine with what to fill the screen.
    /// </summary>
    private readonly Advance _advance;

    /// <summary>
    /// Creates a new screen to display a part of the story.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    /// <param name="advance">The type of story advance, used to determine with what to fill the screen.</param>
    public StoryScreen(MainWindow main, Advance advance)
    {
        _main = main;
        InitializeComponent();

        _advance = advance;

        switch (_advance)
        {
            case Advance.Intro i:
                LoadScreen(i);
                break;
            case Advance.FirstMurder fm:
                LoadScreen(fm);
                break;
            case Advance.PreDeath pred:
                LoadScreen(pred);
                break;
            case Advance.PostDeath postd:
                LoadScreen(postd);
                break;
            case Advance.PostAccusation pa:
                LoadScreen(pa);
                break;
            default:
                throw new ArgumentException($"Unknown advance type: {advance.GetType()}", nameof(advance));
        }
    }

    /// <summary>
    /// Loads the screen with the introduction text.
    /// </summary>
    private void LoadScreen(Advance.Intro _)
    {
        Header.Content    = "Welcome Aboard!";
        MainText.Text     = _main.State.Story.Intro;
        NextButton.Click +=
            async (s, e) => await _main.LoadCharacterIntroNumber(0);
    }
    
    /// <summary>
    /// Loads the screen with the text before the first murder.
    /// </summary>
    private void LoadScreen(Advance.FirstMurder _)
    {
        Header.Content    = "Welcome Aboard!";
        MainText.Text     = _main.State.Story.FirstMurder;
        NextButton.Click +=
            async (s, e) => await _main.AdvanceStory(new Advance.PreDeath());
    }
    
    /// <summary>
    /// Loads the screen with the information before the death of a character.
    /// </summary>
    /// <param name="advance">The Advance, possibly containing the name of a specific victim.</param>
    private void LoadScreen(Advance.PreDeath advance)
    {
        Victim victim;
        if (advance.Victim is string who)
        {
            Validator.ValidateCharacter(who, _main.State);
            victim = new Victim.ByName.Innocent(who);
        }
        else victim = new Victim.Random();
        _main.State.KillCharacter(victim, out who);

        Character deadChar = _main.State.Characters[who];
        Header.Content     = $"Poor {deadChar.ShortName}...";
        MainText.Text      = deadChar.PreDeathBlurb;
        NextButton.Click  += async (s, e) =>
            await _main.LoadCrimeSceneFor(who);
    }
    
    /// <summary>
    /// Loads the screen with the information after the death of a character.
    /// </summary>
    private void LoadScreen(Advance.PostDeath _)
    {
        Character deadChar = _main.State.Characters[_main.State.LastVictim];
        Header.Content     = $"We'll miss you, {deadChar.ShortName}.";
        MainText.Text      = deadChar.PostDeathBlurb;
        NextButton.Click  += async (s, e) =>
            await _main.AdvanceStory(new Advance.PreDeath());
    }
    
    /// <summary>
    /// Loads the screen with the information after an accusation has been made.
    /// </summary>
    /// <param name="advance">The Advance that contains the information regarding the accusation.</param>
    private void LoadScreen(Advance.PostAccusation advance)
    {
        int numLivingGuilty   = _main.State.Characters.GetGuiltyNames(includeDead:   false).Count;
        int numLivingInnocent = _main.State.Characters.GetLivingNames(includeGuilty: false).Count;

        if (numLivingGuilty == 0)
        { ShowVictory(); }
        else if (numLivingGuilty >= numLivingInnocent)
        { ShowDefeat(); }
        else if (advance.Vote is Vote.None)
        { LoadScreen(new Advance.PostDeath()); }
        else if (advance.Vote.Success)
        { ShowGuilty(advance.Vote.Voted); }
        else
        { ShowInnocent(advance.Vote.Voted); }
    }
    
    /// <summary>
    /// Shows the victory screen, which is displayed when the player has won the game.
    /// </summary>
    private void ShowVictory()
    {
        Header.Content    = "You Win!";
        MainText.Text     = _main.State.Story.Victory;
        NextButton.Click += async (s, e) =>
            await _main.ChangeView(new Screen.End(Victory: true));
        GameSaveManager.RemoveGameSaveAsync();
    }
    
    /// <summary>
    /// Shows the defeat screen, which is displayed when the player has lost the game.
    /// </summary>
    private void ShowDefeat()
    {
        Header.Content    = "You lose!";
        MainText.Text     = _main.State.Story.Defeat;
        NextButton.Click += async (s, e) =>
            await _main.ChangeView(new Screen.End(Victory: false));
        GameSaveManager.RemoveGameSaveAsync();
    }
    
    /// <summary>
    /// Shows the screen that displays the result of an accusation, when the accused character was guilty.
    /// </summary>
    /// <param name="name">The name of the guilty character.</param>
    private void ShowGuilty(string name)
    {
        Header.Content    = "Getting There!";
        MainText.Text     = $"{name} was guilty.";
        NextButton.Click += async (s, e) =>
            await _main.AdvanceStory(new Advance.PostDeath());
    }
    
    /// <summary>
    /// Shows the screen that displays the result of an accusation, when the accused character was innocent.
    /// </summary>
    /// <param name="name">The name of the innocent character.</param>
    private void ShowInnocent(string name)
    {
        Header.Content    = "Ouch.";
        MainText.Text     = $"{name} was not guilty.";
        NextButton.Click += async (s, e) =>
            await _main.AdvanceStory(new Advance.PostDeath());
    }
    
    /// <summary>
    /// Gets the data needed to save the current state of the story.
    /// </summary>
    /// <returns>The advance type (and possibly the name of the character about which the story screen is).</returns>
    public (string, string?) GetSaveData()
    {
        string advanceType = _advance.GetType().Name
            ?? throw new InvalidOperationException("Advance type is not set");
        string? name = null;
        if (_advance is Advance.PostAccusation pa)
        { name = pa.Vote.Voted; }
        else if (_advance is Advance.PreDeath)
        { name = _main.State.LastVictim; }
        return (advanceType, name);
    }
}
