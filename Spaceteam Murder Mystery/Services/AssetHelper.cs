namespace SMM.Services;

/// <summary>
/// A helper class intended to work within the project directory and return valid filepaths.
/// </summary>
public static class AssetHelper
{
    /// <summary>
    /// The directory containing all of the assets used for the applciation.
    /// </summary>
    public static string AssetDirectory { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
    
    /// <summary>
    /// The path to the nebula background image used on multiple screens.
    /// </summary>
    public static BitmapImage NebulaBG { get; } = new(new Uri($"{AssetDirectory}/Images/Nebula-Background.png"));
    
    /// <summary>
    /// The path to the spae ship background image used on multiple screens.
    /// </summary>
    public static BitmapImage ShipBG { get; } = new(new Uri($"{AssetDirectory}/Images/Wall-Dimmed.png"));
}