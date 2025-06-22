namespace SMM.Views.Dynamic;

public partial class StoryScreen : UserControl
{
    private readonly MainWindow _main;

    public StoryScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
        NextButton.Click += Intro_Click;
        LoadScreen(firstLoad: true);
    }

    public void Intro_Click(object sender, RoutedEventArgs e)
    {
        NextButton.Click -= Intro_Click;
        NextButton.Click += Next_Click;
        _main.AdvanceStory();
    }
    public void Next_Click(object sender, RoutedEventArgs e)
    {
        _main.State.KillCharacter(new Victim.Random(), out string who);
        _main.LoadCrimeSceneFor(who);
    }

    public void LoadScreen(bool firstLoad = false)
    {
        if (firstLoad)
        { ShowIntro(); }
        else
        {
            int numLiving = _main.State.Characters.GetLivingNames(includeGuilty: true).Count;
            ShowMurder(numLiving);
        }
    }
    private void ShowIntro()
    {
        Header.Content = "Introduction";
        MainText.Text = _main.State.Story.Intro;
    }

    private void ShowMurder(int numLiving)
    {
        Header.Content = "Ouch.";
        
        if (numLiving == 8)
        { MainText.Text = _main.State.Story.FirstMurder; }
        else
        { MainText.Text = $"And then there were {numLiving}."; }
    }
}
