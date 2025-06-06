namespace SMM.Services.DynamicXAML;

using Properties;
public static class AppSettings
{
    /// <summary>
    /// Saves the current window state, size, and position to the application settings.
    /// </summary>
    /// <param name="window">The window from which to save settings.</param>
    public static void Save(Window window)
    {
        if (window.WindowState == WindowState.Normal)
        {
            Settings.Default.WindowWidth = window.Width;
            Settings.Default.WindowHeight = window.Height;
            Settings.Default.WindowTop = window.Top;
            Settings.Default.WindowLeft = window.Left;
        }

        Settings.Default.WindowState = window.WindowState.ToString();
        Settings.Default.WindowStyle = window.WindowStyle.ToString();
        Settings.Default.ResizeMode = window.ResizeMode.ToString();
        Settings.Default.Save();
    }

    /// <summary>
    /// Loads the window state, size, and position from the application settings.
    /// </summary>
    /// <param name="window">The window on which to apply settings.</param>
    public static void Load(Window window)
    {
        if (Settings.Default.WindowWidth > 0 &&
            Settings.Default.WindowHeight > 0)
        {
            window.Width = Settings.Default.WindowWidth;
            window.Height = Settings.Default.WindowHeight;
        }

        window.Top = Settings.Default.WindowTop;
        window.Left = Settings.Default.WindowLeft;

        if (Enum.TryParse(Settings.Default.WindowState, out WindowState state))
        { window.WindowState = state; }

        if (Enum.TryParse(Settings.Default.WindowStyle, out WindowStyle style))
        { window.WindowStyle = style; }

        if (Enum.TryParse(Settings.Default.ResizeMode, out ResizeMode resize))
        { window.ResizeMode = resize; }
    }
}