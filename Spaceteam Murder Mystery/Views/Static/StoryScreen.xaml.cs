namespace SMM.Views.Static;

public partial class StoryScreen : UserControl
{
    private readonly MainWindow _main;

    public StoryScreen(MainWindow main, Advance advance)
    {
        _main = main;
        InitializeComponent();

        switch (advance)
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

    private void LoadScreen(Advance.Intro _)
    {
        Header.Content = "Welcome Aboard!";
        MainText.Text = _main.State.Story.Intro;
        NextButton.Click +=
            async (s, e) => await _main.AdvanceStory(new Advance.FirstMurder());
    }
    private void LoadScreen(Advance.FirstMurder _)
    {
        Header.Content = "Welcome Aboard!";
        MainText.Text = _main.State.Story.FirstMurder;
        NextButton.Click +=
            async (s, e) => await _main.AdvanceStory(new Advance.PreDeath());
    }
    private void LoadScreen(Advance.PreDeath _)
    {
        _main.State.KillCharacter(new Victim.Random(), out string who);
        Character victim = _main.State.Characters[who];
        Header.Content = $"Poor {victim.ShortName}...";
        MainText.Text = victim.PreDeathBlurb;
        NextButton.Click += async (s, e) =>
            await _main.LoadCrimeSceneFor(who);
    }
    private void LoadScreen(Advance.PostDeath _)
    {
        Character victim = _main.State.Characters[_main.State.LastVictim];
        Header.Content = $"Poor {victim.ShortName}...";
        MainText.Text = victim.PostDeathBlurb;
        NextButton.Click += async (s, e) =>
            await _main.AdvanceStory(new Advance.PreDeath());
    }
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
    private void ShowVictory()
    {
        Header.Content    = "You Win!";
        MainText.Text     = _main.State.Story.Victory;
        NextButton.Click += async (s, e) =>
            await _main.ChangeView(new Screen.End(Victory: true));
    }
    private void ShowDefeat()
    {
        Header.Content    = "You lose!";
        MainText.Text     = _main.State.Story.Defeat;
        NextButton.Click += async (s, e) =>
            await _main.ChangeView(new Screen.End(Victory: false));
    }
    private void ShowGuilty(string name)
    {
        Header.Content    = "Getting There!";
        MainText.Text     = $"{name} was guilty.";
        NextButton.Click += async (s, e) =>
            await _main.AdvanceStory(new Advance.PostDeath());
    }
    private void ShowInnocent(string name)
    {
        Header.Content    = "Ouch.";
        MainText.Text     = $"{name} was not guilty.";
        NextButton.Click += async (s, e) =>
            await _main.AdvanceStory(new Advance.PostDeath());
    }
}
