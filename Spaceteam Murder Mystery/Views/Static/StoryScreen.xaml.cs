namespace SMM.Views.Static;

public partial class StoryScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly bool _firstLoad;

    public StoryScreen(MainWindow main, bool firstLoad, Vote? vote)
    {
        InitializeComponent();
        _main = main;
        _firstLoad = firstLoad;
        if (_firstLoad)
        {
            ShowIntro();
            NextButton.Click +=
                async (s, e) => await _main.AdvanceStory();
        }
        else
        {
            NextButton.Click += async (s, e) =>
            {
                _main.State.KillCharacter(new Victim.Random(), out string who);
                await _main.LoadCrimeSceneFor(who);
            };
            int numLiving = _main.State.Characters.GetLivingNames(includeGuilty: true).Count;
            ShowDeath(numLiving, vote);
        }
    }

    private void ShowIntro()
    {
        Header.Content = "Introduction";
        MainText.Text = _main.State.Story.Intro;
    }

    private void ShowDeath(int numLiving, Vote? vote)
    {
        Header.Content = "Ouch.";
        
        if (numLiving == 8)
        { MainText.Text = _main.State.Story.FirstMurder; }
        else
        {
            if (vote is null)
            { throw new ArgumentException("If one or more characters are dead, vote cannot be null.", nameof(vote)); }
            MainText.Text = $"And then there were {numLiving}.";
        }
    }
}
