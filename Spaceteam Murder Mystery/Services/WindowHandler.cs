namespace SMM.Services;

/// <summary>
/// A wrapper for all of the data pertaining to the current state of the app window.
/// </summary>
public partial class WindowHandler(MainWindow window)
{
    /// <summary>
    /// System metric to ensure that the window is shown after resizing.
    /// </summary>
    private const uint SWP_SHOWWINDOW = 0x0040;
    
    /// <summary>
    /// System metric for the primary screen width.
    /// </summary>
    private const int SM_CXSCREEN = 0;
    
    /// <summary>
    /// System metric for the primary screen height.
    /// </summary>
    private const int SM_CYSCREEN = 1;

    /// <summary>
    /// The handle to the main window.
    /// </summary>
    private IntPtr HWND { get; } = new WindowInteropHelper(window).Handle;
    
    /// <summary>
    /// Constant for setting the window as topmost.
    /// </summary>
    private static IntPtr HWND_TOPMOST { get; } = new IntPtr(-1);

    /// <summary>
    /// Constant for removing the topmost status from the window.
    /// </summary>
    private static IntPtr HWND_NOTOPMOST { get; } = new IntPtr(-2);

    // =============== "Previous" window state properties =============== //

    /// <summary>
    /// Stores the previous window state before entering fullscreen.
    /// </summary>
    private WindowState PrevWindowState { get; set; } = WindowState.Normal;

    /// <summary>
    /// Stores the previous window style before entering fullscreen.
    /// </summary>
    private WindowStyle PrevWindowStyle { get; set; } = WindowStyle.SingleBorderWindow;
    
    /// <summary>
    /// Stores the previous resize mode before entering fullscreen.
    /// </summary>
    private ResizeMode PrevResizeMode { get; set; } = ResizeMode.CanResize;
    
    /// <summary>
    /// Stores whether the window was previously topmost.
    /// </summary>
    private bool PrevTopmost { get; set; } = false;
    
    /// <summary>
    /// Stores the previous width of the window before entering fullscreen.
    /// </summary>
    private double PrevWidth { get; set; } = 854;
    
    /// <summary>
    /// Stores the previous height of the window before entering fullscreen.
    /// </summary>
    private double PrevHeight { get; set; } = 480;
    
    /// <summary>
    /// Stores the previous left position of the window before entering fullscreen.
    /// </summary>
    private double PrevLeft { get; set; } = 100;
    
    /// <summary>
    /// Stores the previous top position of the window before entering fullscreen.
    /// </summary>
    private double PrevTop { get; set; } = 100;

    // ================================================================== //

    /// <summary>
    /// The main window of the application.
    /// </summary>
    public MainWindow Win { get; } = window;
    
    /// <summary>
    /// Indicates whether the window is currently in fullscreen mode.
    /// </summary>
    public bool IsFullScreen { get; set; } = false;

    /// <summary>
    /// Whether or not to pause the soundtrack when the main window loses focus.
    /// </summary>
    public bool PauseOnLoseFocus { get; set; } = true;

    /// <summary>
    /// Handles the event when the main window loses focus
    /// (e.g., Alt+Tab to another app).
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    public void MainWindow_Deactivated(object? sender, EventArgs e)
    {
        if (IsFullScreen)
            SetTopmostAndBounds(HWND, false);
        if (PauseOnLoseFocus)
            Win.Soundtrack.Pause();
    }

    /// <summary>
    /// Handles the event when the main window regains focus.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments.</param>
    public void MainWindow_Activated(object? sender, EventArgs e)
    {
        if (IsFullScreen)
            SetTopmostAndBounds(HWND, true);
        if (PauseOnLoseFocus)
            Win.Soundtrack.Resume();
    }
    
    /// <summary>
    /// Toggles the window between fullscreen and windowed mode.
    /// </summary>
    public void ToggleFullScreen()
    {
        if (IsFullScreen) ExitFullScreen();
        else EnterFullScreen();
    }

    /// <summary>
    /// Sets the window's Topmost property and
    /// resizes it to cover the screen if needed.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="makeTopmost">Whether to set the window as topmost.</param>
    private void SetTopmostAndBounds(IntPtr hwnd, bool makeTopmost)
    {
        // We still want it to cover the same rectangle we originally sized it to.
        // So grab the current Width/Height in *physical pixels*.
        int screenWidthPx  = GetSystemMetrics(SM_CXSCREEN);
        int screenHeightPx = GetSystemMetrics(SM_CYSCREEN);

        // Choose the right "insert‐after" constant.
        IntPtr insertAfter = makeTopmost ? HWND_TOPMOST : HWND_NOTOPMOST;

        SetWindowPos(
            hwnd,
            insertAfter,
            0, 0,
            screenWidthPx,
            screenHeightPx,
            SWP_SHOWWINDOW
        );

        // Also sync WPF’s Topmost property.
        Win.Topmost = makeTopmost;
    }

    /// <summary>
    /// Switches the window to fullscreen mode,
    /// saving previous window state and style.
    /// </summary>
    private void EnterFullScreen()
    {
        if (IsFullScreen) return;

        // Save "before" values.
        PrevWindowState = Win.WindowState;
        PrevWindowStyle = Win.WindowStyle;
        PrevResizeMode  = Win.ResizeMode;
        PrevTopmost     = Win.Topmost;
        PrevWidth       = Win.Width;
        PrevHeight      = Win.Height;
        PrevLeft        = Win.Left;
        PrevTop         = Win.Top;

        // Remove chrome and prevent resizing.
        Win.WindowState = WindowState.Normal;
        Win.WindowStyle = WindowStyle.None;
        Win.ResizeMode  = ResizeMode.NoResize;

        // Grab the HWND for SetWindowPos.
        nint hwnd = new WindowInteropHelper(Win).Handle;

        // Determine "primary screen" dimensions in WPF units (DIPs).
        int screenWidthPx  = GetSystemMetrics(SM_CXSCREEN);
        int screenHeightPx = GetSystemMetrics(SM_CYSCREEN);

        // Move/resize window to (0, 0)-(screenWidth, screenHeight), set Topmost.
        SetWindowPos(
            hwnd,
            HWND_TOPMOST,
            0, 0,
            screenWidthPx,
            screenHeightPx,
            SWP_SHOWWINDOW
        );

        Win.Topmost  = true;
        IsFullScreen = true;
    }

    /// <summary>
    /// Exits fullscreen mode and restores
    /// the previous window state, style, and position.
    /// </summary>
    private void ExitFullScreen()
    {
        if (!IsFullScreen) return;

        // Restore size & position.
        Win.Width  = PrevWidth;
        Win.Height = PrevHeight;
        Win.Left   = PrevLeft;
        Win.Top    = PrevTop;

        // Restore state, style, resize mode, and topmost.
        Win.WindowState = PrevWindowState;
        Win.WindowStyle = PrevWindowStyle;
        Win.ResizeMode  = PrevResizeMode;
        Win.Topmost     = PrevTopmost;

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
        int X,  int Y,
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