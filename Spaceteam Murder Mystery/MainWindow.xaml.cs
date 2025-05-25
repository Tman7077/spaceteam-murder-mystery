using System.Windows;
using System.Windows.Controls;
using SMM.Views;

namespace SMM
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Func<UserControl>> _viewMap;
        public MainWindow()
        {
            InitializeComponent();
            _viewMap = new Dictionary<string, Func<UserControl>>
            {
                { "Title", () => new TitleScreen(this) },
                { "Settings", () => new SettingsScreen(this) }
                // { "Game", () => new GameScreen(this) } // Need to implement the choice for GameScreen difficulty.
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
}