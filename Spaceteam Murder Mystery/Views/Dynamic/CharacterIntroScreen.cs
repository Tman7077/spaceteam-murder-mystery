using System.Windows.Navigation;

namespace SMM.Views.Dynamic;

public partial class CharacterIntroScreen : InspectionScreen
{
    private readonly Character _character;
    private readonly int      _index;
    public CharacterIntroScreen(MainWindow main, string name, int i) : base(main)
    {
        Character character = main.State.Characters[name];
        _character = character;
        _index     = i;
        _dir = character.Facing switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"Unknown character facing direction: {character.Facing}")
        };

        LoadScreen();
    }

    protected override void LoadScreen()
    {
        Grid root = LoadScreenSetup();

        TextBlock mottoBlock = new()
        {
            Style               = (Style)FindResource("BodyTextBlock"),
            Foreground          = (Brush)FindResource("SMMOrange"),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center,
            TextWrapping        = TextWrapping.Wrap,
            Text                = _character.Motto
        };
        mottoBlock.SetBinding(FontSizeProperty, FontScaleExtension.GetBinding(0.75));

        Grid block = CreateImageGrid(_character.ProfileUri, _character.Name, mottoBlock);

        TextBlock text = new()
        {
            Style = (Style)FindResource("BodyTextBlock"),
            Text  = _character.Description,
        };

        Continue_Click continueClick;
        if (_index + 1 == _main.State.Characters.Count)
            continueClick = async (s, e) => await _main.AdvanceStory(new Advance.FirstMurder());
        else
            continueClick = async (s, e) => await _main.LoadCharacterIntroNumber(_index + 1);
            
        LoadScreenFinal(root, block, text, continueClick);

    }
}