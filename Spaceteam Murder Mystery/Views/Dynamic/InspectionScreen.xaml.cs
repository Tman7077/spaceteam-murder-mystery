namespace SMM.Views.Dynamic;

/// <summary>
/// A screen to display an image and its description.
/// </summary>
public abstract partial class InspectionScreen : UserControl
{
    /// <summary>
    /// An async method to assign to the continue button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    /// <returns></returns>
    protected delegate Task Continue_Click(object sender, RoutedEventArgs e);

    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    protected readonly MainWindow _main;

    /// <summary>
    /// The side of the screen on which the desription is displayed.
    /// </summary>
    protected Direction _dir;

    /// <summary>
    /// Creates a screen to display an image and its description.
    /// </summary>
    /// <param name="main">The MainWindow of the application.</param>
    protected InspectionScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();
    }

    /// <summary>
    /// Loads the screen with the appropriate content based on the image being inspected.
    /// </summary>
    protected abstract void LoadScreen();

    /// <summary>
    /// Provides the always-present items to display.
    /// </summary>
    /// <returns>A Grid laid out as necessary to be filled.</returns>
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

    /// <summary>
    /// Completes the screen setup.
    /// </summary>
    /// <param name="root">The main grid container.</param>
    /// <param name="block">The grid containing the image and label.</param>
    /// <param name="text">The text block (description) to show.</param>
    /// <param name="continueClick">The method to apply to the click button.</param>
    protected void LoadScreenFinal(Grid root, Grid block, TextBlock text, Continue_Click continueClick)
    {
        Button button = new()
        {
            Style   = (Style)FindResource("CornerCutButton"),
            Content = "Continue",
            HorizontalAlignment =
                _dir == Direction.Left
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left
        };
        button.SetBinding(WidthProperty, DivideWindowWidthExtension.GetBinding(10));
        button.Click += async (sender, e) => await continueClick(sender, e);

        int[] colAssignments = _dir switch
        {
            Direction.Left  => [1, 3, 4],
            Direction.Right => [4, 1, 1],
            _ => throw new ArgumentException($"Unknown direction: {_dir}")
        };

        Grid.SetColumnSpan(text, 2);

        Grid.SetColumn(block,  colAssignments[0]);
        Grid.SetColumn(text,   colAssignments[1]);
        Grid.SetColumn(button, colAssignments[2]);

        Grid.SetRow(block,  2);
        Grid.SetRow(text,   2);
        Grid.SetRow(button, 3);

        root.Children.Add(block);
        root.Children.Add(text);
        root.Children.Add(button);

        Content = root;
    }

    /// <summary>
    /// Creates a grid to display an image, its name, and an optional motto.
    /// </summary>
    /// <param name="imgUri">The path to the image to display.</param>
    /// <param name="name">The name of the item/character.</param>
    /// <param name="mottoBlock">Optionally, the motto of the character.</param>
    /// <returns>A completed Grid with the image, label, and motto(?).</returns>
    protected Grid CreateImageGrid(Uri imgUri, string name, TextBlock? mottoBlock = null)
    {
        Grid imgGrid = new();

        imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        imgGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        Image image = new()
        { Source = new BitmapImage(imgUri) };

        Label nameLabel = new()
        {
            Style   = (Style)FindResource("CornerCutLabel"),
            Content = name
        };

        Grid.SetRow(image,     0);
        Grid.SetRow(nameLabel, 2);

        imgGrid.Children.Add(image);
        imgGrid.Children.Add(nameLabel);

        if (mottoBlock is not null)
        {
            Grid.SetRow(mottoBlock, 1);
            imgGrid.Children.Add(mottoBlock);
        }

        return imgGrid;
    }
}
