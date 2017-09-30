using Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Common.Extensions;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.IO;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class HyperspinBackup
    {
        private DirectoryInfo _di;

        public string SystemName { get; set; }

        public HyperspinBackup(string hyperspinPath, string systemName)
        {
            _di = new DirectoryInfo(hyperspinPath);
            SystemName = systemName;
        }

        /// <summary>
        /// Backups a systems media folder. 
        /// </summary>
        /// <param name="destPath">The dest path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public void BackupMediaFolder(string destPath, bool overwrite)
        {
            //Get the systems media folder location
            var mediaFolder = Path.Combine(_di.FullName, HyperspinRootPaths.Media, SystemName);

            //Create Dinfo to use extension
            var dInfo = new DirectoryInfo(mediaFolder);
            
            var path = Path.Combine(destPath, "Hyperspin", "Media", SystemName);
            
            dInfo.CopyAllDirectoriesAndFiles(path , overwrite);
        }

        public void BackupType(string destPath, HsMediaType hsMediaType, bool overwrite)
        {
            var dir = destPath;

            switch (hsMediaType)
            {
                case HsMediaType.Artwork1:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Artwork1);
                    break;
                case HsMediaType.Artwork2:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Artwork2);
                    break;
                case HsMediaType.Artwork3:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Artwork3);
                    break;
                case HsMediaType.Artwork4:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Artwork4);
                    break;
                case HsMediaType.Backgrounds:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Backgrounds);
                    break;
                case HsMediaType.Wheel:
                    dir = HyperspinPaths.GetMediaDirectory(
                        _di.FullName, SystemName, HsMediaType.Wheel);
                    break;
                case HsMediaType.Video:
                    break;
                default:
                    return;
            }

            RunBackup(_di.FullName, dir, overwrite);
        }        

        private void RunBackup(string dir, string destPath, bool overwrite)
        {
            var mediaPath = new DirectoryInfo(dir);

            mediaPath.CopyFiles(destPath, overwrite);
        }
    }

    public enum BackupType
    {
        All
    }
}
