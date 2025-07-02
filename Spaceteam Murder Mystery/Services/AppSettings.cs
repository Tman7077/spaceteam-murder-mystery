namespace SMM.Services;

using Properties;

public static class AppSettings
{
    public static double Volume
    {
        get => Settings.Default.Volume;
        set
        {
            if (value < 0 || value > 1)
            { throw new ArgumentOutOfRangeException(nameof(value), "Volume must be between 0 and 1"); }
            Settings.Default.Volume = value;
            Settings.Default.Save();
        }
    }
    public static bool Muted
    {
        get => Settings.Default.Muted;
        set
        {
            Settings.Default.Muted = value;
            Settings.Default.Save();
        }
    }
    
    /// <summary>
    /// Saves the current window state, size, and position to the application settings.
    /// </summary>
    /// <param name="handler">The window handler from which to save settings.</param>
    public static void Save(WindowHandler handler)
    {
        Window window = handler.Win;
        if (handler.IsFullScreen)
        {
            Settings.Default.WindowWidth  = 854;
            Settings.Default.WindowHeight = 480;
            Settings.Default.WindowLeft   = 100;
            Settings.Default.WindowTop    = 100;
        }
        else if (window.WindowState == WindowState.Normal)
        {
            Settings.Default.WindowWidth  = window.Width;
            Settings.Default.WindowHeight = window.Height;
            Settings.Default.WindowLeft   = window.Left;
            Settings.Default.WindowTop    = window.Top;
        }

        Settings.Default.WindowState      = window.WindowState.ToString();
        Settings.Default.WindowStyle      = window.WindowStyle.ToString();
        Settings.Default.ResizeMode       = window.ResizeMode.ToString();
        Settings.Default.IsFullScreen     = handler.IsFullScreen;
        Settings.Default.PauseOnLoseFocus = handler.PauseOnLoseFocus;
        Settings.Default.Save();
    }

    /// <summary>
    /// Loads the window state, size, and position from the application settings.
    /// </summary>
    /// <param name="handler">The window handler to which to apply settings.</param>
    public static void Load(WindowHandler handler, out bool fullscreen)
    {
        if (Settings.Default.IsFullScreen)
        {
            fullscreen = true;
            return;
        }
        
        fullscreen    = false;
        Window window = handler.Win;

        if (Settings.Default.WindowWidth  > 0 &&
            Settings.Default.WindowHeight > 0)
        {
            window.Width  = Settings.Default.WindowWidth;
            window.Height = Settings.Default.WindowHeight;
        }

        window.Left = Settings.Default.WindowLeft;
        window.Top  = Settings.Default.WindowTop;

        if (Enum.TryParse(Settings.Default.WindowState, out WindowState state))
        { window.WindowState = state; }

        if (Enum.TryParse(Settings.Default.WindowStyle, out WindowStyle style))
        { window.WindowStyle = style; }

        if (Enum.TryParse(Settings.Default.ResizeMode,  out ResizeMode resize))
        { window.ResizeMode  = resize; }
        
        handler.PauseOnLoseFocus = Settings.Default.PauseOnLoseFocus;
    }
}