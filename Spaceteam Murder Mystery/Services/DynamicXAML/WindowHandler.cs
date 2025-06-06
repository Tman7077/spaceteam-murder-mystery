namespace SMM.Services.DynamicXAML;

/// <summary>
/// A wrapper for all of the data pertaining to the current state of the app window.
/// </summary>
/// <param name="prevWindowState">The previous window mode (minimized, maximized, etc.).</param>
/// <param name="prevWindowStyle">The previous window chrome.</param>
/// <param name="prevResizeMode">The previous resizability state.</param>
/// <param name="prevLeft">The previous offset from the left border of the screen.</param>
/// <param name="prevTop">The previous offset from the top of the screen.</param>
/// <param name="prevWidth">The previous width.</param>
/// <param name="prevHeight">The previous height.</param>
public partial class WindowHandler
(
    Window window,
    WindowState prevWindowState,
    WindowStyle prevWindowStyle,
    ResizeMode prevResizeMode,
    double prevLeft, double prevTop,
    double prevWidth, double prevHeight
)
{
    private IntPtr HWND { get; } = new WindowInteropHelper(window).Handle;
    private static IntPtr HWND_TOPMOST { get; } = new IntPtr(-1);
    private static IntPtr HWND_NOTOPMOST { get; } = new IntPtr(-2);
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;
    private Window Win { get; } = window;
    private WindowState PrevWindowState { get; set; } = prevWindowState;
    private WindowStyle PrevWindowStyle { get; set; } = prevWindowStyle;
    private ResizeMode PrevResizeMode { get; set; } = prevResizeMode;
    private bool PrevTopmost { get; set; } = false;
    private double PrevLeft { get; set; } = prevLeft;
    private double PrevTop { get; set; } = prevTop;
    private double PrevWidth { get; set; } = prevWidth;
    private double PrevHeight { get; set; } = prevHeight;
    private bool IsFullScreen { get; set; } = false;

    /// <summary>
    /// Handles the event when the main window loses focus (e.g., Alt+Tab to another app).
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    public void MainWindow_Deactivated(object? sender, EventArgs e)
    {
        if (IsFullScreen)
        { SetTopmostAndBounds(HWND, false); }
    }

    /// <summary>
    /// Handles the event when the main window regains focus.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    public void MainWindow_Activated(object? sender, EventArgs e)
    {
        if (IsFullScreen)
        { SetTopmostAndBounds(HWND, true); }
    }
    
    /// <summary>
    /// Toggles the window between fullscreen and windowed mode.
    /// </summary>
    public void ToggleFullScreen()
    {
        if (IsFullScreen)
        { ExitFullScreen(); }
        else
        { GoFullScreen(); }
    }

    /// <summary>
    /// Sets the window's Topmost property and resizes it to cover the screen if needed.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="makeTopmost">Whether to set the window as topmost.</param>
    private void SetTopmostAndBounds(IntPtr hwnd, bool makeTopmost)
    {
        // We still want it to cover the same rectangle we originally sized it to.
        // So grab the current Width/Height in *physical pixels*. 
        // (If your DPI changes while your app is running, you might need a more complex approach;
        //  but for a steady DPI, GetSystemMetrics is fine.)
        int screenWidthPx = GetSystemMetrics(SM_CXSCREEN);
        int screenHeightPx = GetSystemMetrics(SM_CYSCREEN);

        // Choose the right “insert‐after” constant:
        IntPtr insertAfter = makeTopmost ? HWND_TOPMOST : HWND_NOTOPMOST;

        SetWindowPos(
            hwnd,
            insertAfter,
            0, 0,
            screenWidthPx, screenHeightPx,
            SWP_SHOWWINDOW
        );

        // Also sync WPF’s Topmost property (so that if you ever check `this.Topmost`, it’s correct):
        Win.Topmost = makeTopmost;
    }

    /// <summary>
    /// Switches the window to fullscreen mode, saving previous window state and style.
    /// </summary>
    private void GoFullScreen()
    {
        if (IsFullScreen)
        { return; } // already fullscreen

        // Save “before” values:
        PrevWindowState = Win.WindowState;
        PrevWindowStyle = Win.WindowStyle;
        PrevResizeMode = Win.ResizeMode;
        PrevTopmost = Win.Topmost;
        PrevLeft = Win.Left;
        PrevTop = Win.Top;
        PrevWidth = Win.Width;
        PrevHeight = Win.Height;

        // Remove chrome and prevent resizing:
        Win.WindowStyle = WindowStyle.None;
        Win.ResizeMode = ResizeMode.NoResize;
        Win.WindowState = WindowState.Normal; // we’ll manage size ourselves

        // Grab the HWND for SetWindowPos:
        nint hwnd = new WindowInteropHelper(Win).Handle;

        // Determine “primary screen” dimensions in WPF units (DIPs):
        int screenWidthPx = GetSystemMetrics(SM_CXSCREEN);
        int screenHeightPx = GetSystemMetrics(SM_CYSCREEN);

        // Move/resize window to (0,0)-(screenWidth,screenHeight), set Topmost:
        SetWindowPos(
            hwnd,
            HWND_TOPMOST,
            0, 0,           // X, Y
            screenWidthPx,  // cx, cy
            screenHeightPx, // cy
            SWP_SHOWWINDOW
        );

        Win.Topmost = true;
        IsFullScreen = true;
    }

    /// <summary>
    /// Exits fullscreen mode and restores the previous window state, style, and position.
    /// </summary>
    private void ExitFullScreen()
    {
        if (!IsFullScreen)
        { return; }

        // Restore style, resize mode, and topmost:
        Win.WindowStyle = PrevWindowStyle;
        Win.ResizeMode = PrevResizeMode;
        Win.Topmost = PrevTopmost;

        // Restore size & position
        Win.Left = PrevLeft;
        Win.Top = PrevTop;
        Win.Width = PrevWidth;
        Win.Height = PrevHeight;

        // Restore WindowState
        Win.WindowState = PrevWindowState;

        IsFullScreen = false;
    }

    /// <summary>
    /// P/Invoke to SetWindowPos
    /// (Sets the window position and size using the Win32 API).
    /// </summary>
    /// <param name="hWnd">Handle to the window.</param>
    /// <param name="hWndInsertAfter">Handle to the window to precede the positioned window in the Z order.</param>
    /// <param name="X">New position of the left side of the window.</param>
    /// <param name="Y">New position of the top of the window.</param>
    /// <param name="cx">New width of the window.</param>
    /// <param name="cy">New height of the window.</param>
    /// <param name="uFlags">Window sizing and positioning flags.</param>
    /// <returns>True if successful; otherwise, false.</returns>
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool SetWindowPos
    (
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X, int Y,
        int cx, int cy,
        uint uFlags
    );

    /// <summary>
    /// Retrieves various system metrics or system configuration settings.
    /// </summary>
    /// <param name="nIndex">The system metric or configuration setting to retrieve.</param>
    /// <returns>The requested system metric or configuration setting.</returns>
    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial int GetSystemMetrics(int nIndex);
}