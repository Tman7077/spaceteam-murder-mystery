namespace SMM.Views.Static;

using System.Windows.Media.Animation;

public partial class TitleScreen : UserControl
{
    private readonly MainWindow _main;

    public TitleScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
        Background = new ImageBrush()
        {
            ImageSource = AssetHelper.NebulaBG,
            Stretch = Stretch.UniformToFill
        };
        AnimateTitleGradient();
    }

    private async void StartGame_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Difficulty());

    private async void ContinueGame_Click(object sender, RoutedEventArgs e) =>
        await _main.LoadGame();

    private async void Settings_Click(object sender, RoutedEventArgs e) =>
        await _main.ChangeView(new Screen.Settings(), 0, 0);

    private void Quit_Click(object sender, RoutedEventArgs e) =>
        Application.Current.Shutdown();

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

        // RotateTransform    rt    = new(30, 0.5, 0.5);
        TranslateTransform tt    = new();
        // TransformGroup     group = new();
        // group.Children.Add(rt);
        // group.Children.Add(tt);

        // gradient.RelativeTransform = group;
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