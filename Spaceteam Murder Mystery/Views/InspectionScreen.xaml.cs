namespace SMM.Views;

public abstract partial class InspectionScreen : UserControl
{
    protected readonly MainWindow _main;
    protected Direction _dir;

    protected InspectionScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }
    protected abstract void LoadScreen();

    protected (Grid, bool) LoadScreenSetup()
    {
        Grid root = new();

        int[] colDefs = (string)_dir switch
        {
            "Left"  => [1, 2, 1, 4, 1],
            "Right" => [1, 4, 1, 2, 1],
            _ => throw new ArgumentException($"Unknown direction: {_dir}")
        };

        foreach (int width in colDefs)
        { root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width, GridUnitType.Star) }); }

        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        
        return (root, _dir == "Left");
    }
    protected void LoadScreenFinal(Grid root, Grid block, TextBlock text)
    {
        Grid.SetRow(block, 1);
        Grid.SetRow(text,  1);

        root.Children.Add(block);
        root.Children.Add(text);

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
