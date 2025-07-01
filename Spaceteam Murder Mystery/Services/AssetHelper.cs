namespace SMM.Services;

using System.IO;

/// <summary>
/// A helper class intended to work within the project directory and return valid filepaths.
/// </summary>
public static class AssetHelper
{
    public static string      AssetDirectory { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
    public static BitmapImage NebulaBG       { get; } = new(new Uri($"{AssetDirectory}/Images/Nebula-Background.png"));
    public static BitmapImage ShipBG         { get; } = new(new Uri($"{AssetDirectory}/Images/Wall-Dimmed.png"));
}