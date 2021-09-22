using System.IO;

namespace NoitaTool.Extensions
{
    public static class DirectoryInfoExtension
    {
        public static void Copy(this DirectoryInfo sourceDir, string destDirName, bool copySubDirs)
        {
            if(!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Directory not found: {sourceDir.FullName}");
            }

            DirectoryInfo[] dirs = sourceDir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sourceDir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    subdir.Copy(tempPath, copySubDirs);
                }
            }
        }
    }
}
