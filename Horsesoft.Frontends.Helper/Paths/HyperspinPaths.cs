﻿
using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.IO;

namespace Horsesoft.Frontends.Helper.Paths.Hyperspin
{
    public static class HyperspinPaths
    {
        /// <summary>
        /// Returns the full path for the given mediatype with the system set to this.
        /// </summary>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        public static string GetMediaDirectory(string frontendPath, string systemName, HsMediaType mediaType)
        {
            switch (mediaType)
            {
                case HsMediaType.Artwork1:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Artwork1);
                case HsMediaType.Artwork2:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Artwork2);
                case HsMediaType.Artwork3:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Artwork3);
                case HsMediaType.Artwork4:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Artwork4);
                case HsMediaType.Backgrounds:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Backgrounds);
                case HsMediaType.Wheel:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, Images.Wheels);
                case HsMediaType.Video:
                    return Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, HyperspinRootPaths.Video);
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets a systems database path.
        /// </summary>
        /// <param name="frontendPath">The frontend path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        public static string GetSystemDatabasePath(string frontendPath, string systemName) => Path.Combine(frontendPath, HyperspinRootPaths.Databases, systemName);

        /// <summary>
        /// Gets the media files for game.
        /// </summary>
        /// <param name="frontendPath">The frontend path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="mediaPath">The media path.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetMediaFilesForGame(string frontendPath, string systemName, string mediaPath, string filter = "*.*")
        {            
            var pathToScan = Path.Combine(frontendPath, HyperspinRootPaths.Media, systemName, mediaPath);

            var mediaFiles = new List<string>();
            if (Directory.Exists(pathToScan))
            {
                mediaFiles.AddRange(Directory.EnumerateFiles(pathToScan, filter));
            }

            return mediaFiles;
        }
    }

    public static class HyperspinRootPaths
    {
        public const string Databases = "Databases";
        public const string Media = "Media";
        public const string Settings = "Settings";
        public const string Sound = "Sound";
        public const string Themes = "Themes";
        public const string Video = "Video";
    }

    public static class Images
    {
        public const string Artwork1 = @"Images\Artwork1";
        public const string Artwork2 = @"Images\Artwork2";
        public const string Artwork3 = @"Images\Artwork3";
        public const string Artwork4 = @"Images\Artwork4";
        public const string Backgrounds = @"Images\Backgrounds";
        public const string GenreWheel = @"Images\Genre\Wheel";
        public const string GenreBackgrounds = @"Images\Genre\Backgrounds";
        public const string Letters = @"Images\Letters";
        public const string Pointer = @"Images\Other";
        public const string Special = @"Images\Special";
        public const string Wheels = @"Images\Wheel";
    }

    public static class Sound
    {
        public const string WheelSounds = @"Sound\Wheel Sounds";
        public const string BackgroundMusic = @"Sound\Background Music";
        public const string SystemStart = @"Sound\System Start";
        public const string SystemExit = @"Sound\System Exit";

    }

}
