namespace SMM.Services;

using System.IO;

/// <summary>
/// A helper class intended to work within the project directory and return valid filepaths.
/// <para>
/// This also unifies SMMTests with SMM, even though their base directories are siblings
/// (rather than SMMTests being nested in SMM).
/// </para>
/// </summary>
public static class PathHelper
{
    /// <summary>
    /// Gets the path to the SMM root folder,
    /// denoted by the presence of a ".root" file.
    /// </summary>
    /// <returns>The full path to the root directory.</returns>
    public static string GetProjectRoot()
    {
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string marker      = ".root";

        while (dir != null && !File.Exists(Path.Combine(dir.FullName, marker)))
        { dir = dir.Parent; }

        if (dir == null)
        { throw new DirectoryNotFoundException($"Could not find project root containing '{marker}'."); }

        return Path.Combine(dir.FullName, "Spaceteam Murder Mystery");
    }

    /// <summary>
    /// Gets the path to the SMM Assets directory,
    /// from which files can be loaded.
    /// </summary>
    /// <returns>The full path to the Assets directory.</returns>
    public static string GetAssetDirectory() =>
        Path.Combine(GetProjectRoot(), "Assets");
}