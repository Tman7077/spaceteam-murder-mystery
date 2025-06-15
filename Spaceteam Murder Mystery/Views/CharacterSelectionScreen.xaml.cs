namespace SMM.Views;

public partial class CharacterSelectionScreen : UserControl
{
    private readonly MainWindow _main;
    private readonly string _labelContent;

    private GameState State { get => _main.State; }
    
    public CharacterSelectionScreen(MainWindow main, string type)
    {
        InitializeComponent();
        _main = main;
        _labelContent = type switch
        {
            "Interviews" => "Interview",
            "Accusations" => "Accuse",
            "Voting" => "Vote Out",
            _ => throw new ArgumentException($"Unknown type: {type}")
        };
        DisplayCharacters();
    }

    public void Interview_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string characterName)
        { _main.LoadInterviewFor(characterName); }
    }

    private void DisplayCharacters()
    {
        int i = 0;
        foreach (Character character in State.Characters.Values)
        {
            Grid charGrid = CreateCharacterBlock(character);
            int column = i % 4 + 1;
            int row = i / 4 + 2;
            i++;
            Grid.SetColumn(charGrid, column);
            Grid.SetRow(charGrid, row);
            MainGrid.Children.Add(charGrid);
        }
    }

    private Grid CreateCharacterBlock(Character character)
    {
        Grid charGrid = new();

        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        charGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        Image portrait = new()
        { Source = new BitmapImage(new Uri(character.ProfileImagePath, UriKind.Absolute)) };

        Label nameLabel = new()
        {
            Content = character.Name,
            Style = (Style)FindResource("BodyText"),
            Foreground = (Brush)FindResource("SMMWhite"),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        Button selectButton = new()
        {
            Content = _labelContent,
            Style = (Style)FindResource("CornerCutButton"),
            Tag = character.Name
        };
        selectButton.Click += Interview_Click;
        selectButton.SetBinding(HeightProperty, AspectRatioExtension.GetBinding(0.2));
        selectButton.SetBinding(MarginProperty, ParentBasedMarginExtension.GetBinding(0.1));

        Grid.SetRow(portrait, 0);
        Grid.SetRow(nameLabel, 1);
        Grid.SetRow(selectButton, 2);

        charGrid.Children.Add(portrait);
        charGrid.Children.Add(nameLabel);
        charGrid.Children.Add(selectButton);

        return charGrid;
    }
}
