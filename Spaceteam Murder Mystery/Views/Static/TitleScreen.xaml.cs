namespace SMM.Views.Static;

using System.Windows.Media.Animation;

/// <summary>
/// A screen to display the main/title menu of the game.
/// </summary>
public partial class TitleScreen : UserControl
{
    /// <summary>
    /// The MainWindow of the application.
    /// </summary>
    private readonly MainWindow _main;

    /// <summary>
    /// Creates a new title screen.
    /// </summary>
    /// <param name="main">The MainWindow of the game.</param>
    public TitleScreen(MainWindow main)
    {
        _main = main;
        InitializeComponent();

        Background = new ImageBrush()
        {
            ImageSource = AssetHelper.NebulaBG,
            Stretch     = Stretch.UniformToFill
        };

        AnimateTitleGradient();
    }

    /// <summary>
    /// Handles the click event of the Start Game button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void StartGame_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Difficulty());

    /// <summary>
    /// Handles the click event of the Continue Game button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void ContinueGame_Click(object sender, RoutedEventArgs e) =>
        await _main.LoadGame();

    /// <summary>
    /// Handles the click event of the Settings button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings(), 0, 0);

    /// <summary>
    /// Handles the click event of the Quit button.
    /// </summary>
    /// <param name="sender">The object that called the method.</param>
    /// <param name="e">The arguments with which the method was called.</param>
    private void Quit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

    /// <summary>
    /// Begins the scrolling gradient for the game title.
    /// </summary>
    public void AnimateTitleGradient()
    {
        LinearGradientBrush gradient = new()
        {
            StartPoint    = new Point(0, 0),
            EndPoint      = new Point(1, 0),
            MappingMode   = BrushMappingMode.RelativeToBoundingBox,
            SpreadMethod  = GradientSpreadMethod.Reflect,
            GradientStops =
            [
                new GradientStop((Color)ColorConverter.ConvertFromString("#C25A0C"), 0.0),
                new GradientStop((Color)ColorConverter.ConvertFromString("#BD8C00"), 0.25),
                new GradientStop((Color)ColorConverter.ConvertFromString("#7CADB2"), 0.5),
                new GradientStop((Color)ColorConverter.ConvertFromString("#036784"), 0.75),
                new GradientStop((Color)ColorConverter.ConvertFromString("#001C42"), 1.0),
                new GradientStop((Color)ColorConverter.ConvertFromString("#036784"), 1.25),
                new GradientStop((Color)ColorConverter.ConvertFromString("#7CADB2"), 1.5),
                new GradientStop((Color)ColorConverter.ConvertFromString("#BD8C00"), 1.75),
                new GradientStop((Color)ColorConverter.ConvertFromString("#C25A0C"), 2.0),
            ]
        };

        TranslateTransform tt = new();

        gradient.RelativeTransform = tt;

        TitleTextBlock.Foreground = gradient;

        var animation = new DoubleAnimation
        {
            From           = 0,
            To             = gradient.GradientStops.Max(gs => gs.Offset),
            Duration       = TimeSpan.FromSeconds(15),
            RepeatBehavior = RepeatBehavior.Forever,
        };

        tt.BeginAnimation(TranslateTransform.XProperty, animation);
    }
}