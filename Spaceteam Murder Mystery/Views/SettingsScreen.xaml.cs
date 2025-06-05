namespace SMM.Views;

public partial class SettingsScreen : UserControl
{
    private readonly MainWindow _main;
    public SettingsScreen(MainWindow main)
    {
        InitializeComponent();
        _main = main;
    }
}
