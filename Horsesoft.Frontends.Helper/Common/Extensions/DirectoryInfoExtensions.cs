using System.IO;
using System.Linq;

namespace Horsesoft.Frontends.Helper.Common.Extensions
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Copies all directories, sub directories and files.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="destPath">The dest path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void CopyAllDirectoriesAndFiles(this DirectoryInfo directory, string destPath, bool overwrite)
        {
            CopyFiles(directory, destPath, overwrite);

            var dirs = directory.EnumerateDirectories();
            foreach (var dir in dirs)
            {
                CopyAllDirectoriesAndFiles(dir, destPath + "\\" + dir.Name, overwrite);
            }
        }

        /// <summary>
        /// Copies the files in this directory
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="destPath">The dest path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void CopyFiles(this DirectoryInfo directory, string destPath, bool overwrite)
        {
            var files = Directory.EnumerateFiles(directory.FullName);

            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            foreach (var file in files)
            {
                var destFilename = Path.GetFileName(file);
                var finalPath = Path.Combine(destPath, destFilename);
                var destFileExists = File.Exists(finalPath);

                //Overwrite existing
                if (destFileExists && overwrite)
                    File.Copy(file, finalPath, true);
                //Create new file
                else if (!destFileExists)
                    File.Copy(file, finalPath);
            }
        }

        /// <summary>
        /// Gets the size of the directory. Searches all sub directories. Only file sizes are calculated.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public static long GetDirectorySize(this DirectoryInfo dir)
        {
            var fileInfos = dir.GetFileSystemInfos("*.*", SearchOption.AllDirectories);

            long dirSize = 0;
            foreach (FileInfo file in fileInfos.Where(x => x.Attributes == FileAttributes.Archive))
            {
                dirSize += file.Length;
            }

            return dirSize;
        }
    }
}
