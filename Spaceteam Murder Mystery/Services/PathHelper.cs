using System.IO;

namespace SMM.Services
{
    public static class PathHelper
    {
        public static string GetProjectRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            string marker = ".root";

            while (dir != null && !File.Exists(Path.Combine(dir.FullName, marker)))
            {
                dir = dir.Parent;
            }

            if (dir == null)
            {
                throw new DirectoryNotFoundException($"Could not find project root containing '{marker}'.");
            }

            return Path.Combine(dir.FullName, "Spaceteam Murder Mystery");
        }
        public static string GetAssetDirectory()
        {
            return Path.Combine(GetProjectRoot(), "Assets");
        }
    }
}