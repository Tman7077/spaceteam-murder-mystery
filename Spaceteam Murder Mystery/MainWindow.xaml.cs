namespace SMM;

using Models.Difficulties;
using System.Windows;
using System.Windows.Controls;
using Views;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, Func<UserControl>> _viewMap;
    public MainWindow()
    {
        InitializeComponent();
        _viewMap = new Dictionary<string, Func<UserControl>>
        {
            { "Title", () => new TitleScreen(this) },
            { "Settings", () => new SettingsScreen(this) },
            { "Difficulty", () => new DifficultyScreen(this) },
            { "EasyGame", () => new GameScreen(this, new DEasy()) },
            { "MediumGame", () => new GameScreen(this, new DMedium()) },
            { "HardGame", () => new GameScreen(this, new DHard()) }
        };
        ChangeView("Title");
    }

    public void ChangeView(string viewName)
    {
        if (_viewMap.TryGetValue(viewName, out Func<UserControl>? createView))
        {
            MainContent.Content = createView();
        }
        else
        {
            MessageBox.Show("View not found: " + viewName, "View Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}