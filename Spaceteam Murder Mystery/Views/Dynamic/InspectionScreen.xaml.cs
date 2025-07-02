namespace SMM.Views.Dynamic;

public abstract partial class InspectionScreen : UserControl
{
    protected delegate Task Continue_Click(object sender, RoutedEventArgs e);
    protected readonly MainWindow      _main;
    protected          Direction       _dir;
    protected          Button          _button;
    
    protected InspectionScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }       

    protected abstract void LoadScreen();

    protected Grid LoadScreenSetup()
    {
        Grid root = new();

        int[] colDefs = _dir switch
        {
            Direction.Left  => [1, 2, 1, 2, 2, 1],
            Direction.Right => [1, 2, 2, 1, 2, 1],
            _ => throw new ArgumentException($"Unknown direction: {_dir}")
        };

        foreach (int width in colDefs)
        { root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Star) }); }

        foreach (int height in (int[])[1, 1, 6, 1, 1])
        { root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(height, GridUnitType.Star) }); }

        return root;
    }
    
    protected void LoadScreenFinal(Grid root, Grid block, TextBlock text, Continue_Click continueClick)
    {
        Button button = new()
        {
            Style = (Style)FindResource("CornerCutButton"),
            Content = "Continue",
            HorizontalAlignment =
                _dir == Direction.Left
                ? HorizontalAlignment.Right
                : HorizontalAlignment.Left
        };
        button.SetBinding(WidthProperty, DivideWindowWidthExtension.GetBinding(10));
        button.Click += (sender, e) => continueClick(sender, e);

        int[] colAssignments = _dir switch
        {
            Direction.Left  => [1, 3, 4],
            Direction.Right => [4, 1, 1],
            _ => throw new ArgumentException($"Unknown direction: {_dir}")
        };

        Grid.SetColumnSpan(text, 2);

        Grid.SetColumn(block,  colAssignments[0]);
        Grid.SetColumn(text,   colAssignments[1]);
        Grid.SetColumn(button, colAssignments[2]);;

        Grid.SetRow(block,  2);
        Grid.SetRow(text,   2);
        Grid.SetRow(button, 3);

        root.Children.Add(block);
        root.Children.Add(text);
        root.Children.Add(button);

        Content = root;
    }

    protected Grid CreateImageGrid(Uri imgUri, string name)
    {
        Grid imgGrid = new();

        imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        Image image = new()
        { Source = new BitmapImage(imgUri) };

        Label nameLabel = new()
        {
            Style   = (Style)FindResource("CornerCutLabel"),
            Content = name
        };

        Grid.SetRow(image,     0);
        Grid.SetRow(nameLabel, 1);

        imgGrid.Children.Add(image);
        imgGrid.Children.Add(nameLabel);

        return imgGrid;
    }
}
