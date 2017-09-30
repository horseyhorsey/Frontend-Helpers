using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Horsesoft.Frontends.Helper.Common;
using System.Linq;
using System.IO;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Media;
using Frontends.Models.Hyperspin;
using Frontends.Models;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Auditing
{
    public class HyperspinAudit : IHyperspinAudit
    {
        #region Fields
        private IFrontend _frontEnd;
        private IMediaHelper _mediaHelper;
        #endregion

        public HyperspinAudit(IFrontend frontEnd, IMediaHelper mediaHelper)
        {
            _frontEnd = frontEnd;
            _mediaHelper = mediaHelper;
        }

        public HyperspinAudit(IFrontend frontEnd)
        {
            _frontEnd = frontEnd;
        }

        #region Public Methods

        /// <summary>
        /// Gets the unused media files.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Media helper is null</exception>
        public async Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType hsMediaType)
        {
            if (_mediaHelper == null)
                throw new NullReferenceException("Media helper is null");

            if (gamesList == null || gamesList.Count() == 0)
                throw new NullReferenceException("Games list is null or count is Zero");

            return await _mediaHelper.GetUnusedMediaFilesAsync(gamesList, hsMediaType);
        }

        public async Task<bool> ScanForMediaAsync(IEnumerable<Game> gamesList)
        {
            string FullPath;
            var hsPath = _frontEnd.Path;
            var systemName = gamesList.ElementAt(0).System;
            var listCount = gamesList.Count();

            if (listCount == 0)
                throw new Exception("Trying to scan media but list is empty");

            //Shouldn't be here for main menu??
            if (systemName.Contains("Main Menu"))
                return false;

            return await Task.Run(() =>
            {
                for (int i = 0; i < listCount; i++)
                {
                    string tempPath = "";

                    var currentGameAudit = gamesList.ElementAt(i);

                    if (systemName != "_Default")
                    {
                        tempPath = Path.Combine(hsPath, HyperspinRootPaths.Media, systemName);

                        FullPath = Path.Combine(tempPath, Images.Wheels, currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveWheel = CheckForFile(FullPath);

                        FullPath = Path.Combine(tempPath, Images.Artwork1,
                            currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveArt1 = CheckForFile(FullPath);
                        FullPath = Path.Combine(tempPath, Images.Artwork2,
                            currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveArt2 = CheckForFile(FullPath);
                        FullPath = Path.Combine(tempPath, Images.Artwork3,
                            currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveArt3 = CheckForFile(FullPath);
                        FullPath = Path.Combine(tempPath, Images.Artwork4,
                            currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveArt4 = CheckForFile(FullPath);

                        FullPath = Path.Combine(tempPath, Images.Backgrounds,
                            currentGameAudit.RomName + ".png");
                        currentGameAudit.MenuAudit.HaveBackground = CheckForFile(FullPath);

                        FullPath = Path.Combine(tempPath, Sound.BackgroundMusic,
                            currentGameAudit.RomName + ".mp3");
                        currentGameAudit.MenuAudit.HaveBGMusic = CheckForFile(FullPath);

                        FullPath = Path.Combine(tempPath, HyperspinRootPaths.Themes, currentGameAudit.RomName + ".zip");
                        currentGameAudit.MenuAudit.HaveTheme = CheckForFile(FullPath);

                        currentGameAudit.MenuAudit.HaveVideo = CheckForVideo(tempPath, currentGameAudit.RomName);
                    }
                }

                return true;
            });
        }

        public async Task<bool> ScanMainMenuMediaAsync(IEnumerable<Game> gamesList)
        {
            var tempPath = "";
            var fullPath = "";

            return await Task.Run(() =>
            {
                foreach (var gameMenu in gamesList)
                {
                    foreach (var hsMenuType in Enum.GetNames(typeof(HsMenuMediaType)))
                    {
                        switch (hsMenuType)
                        {
                            case "Letters":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Images.Letters);
                                gameMenu.MenuAudit.HaveLetters = CheckMediaFolderFiles(fullPath, "*.*");
                                break;
                            case "Special":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Images.Special);
                                gameMenu.MenuAudit.HaveSpecial = CheckMediaFolderFiles(fullPath, "*.*");
                                break;
                            case "Wheel":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, "Main Menu");
                                fullPath = Path.Combine(tempPath, Images.Wheels, gameMenu.RomName + ".png");
                                gameMenu.MenuAudit.HaveWheel = CheckForFile(fullPath);
                                break;
                            case "WheelSounds":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Sound.WheelSounds);
                                gameMenu.MenuAudit.HaveWheelSounds = CheckMediaFolderFiles(fullPath, "*.mp3");
                                break;
                            case "Video":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, "Main Menu");
                                gameMenu.MenuAudit.HaveVideo = CheckForVideo(tempPath, gameMenu.RomName);
                                break;
                            case "Theme":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, "Main Menu");
                                fullPath = Path.Combine(tempPath, HyperspinRootPaths.Themes, gameMenu.RomName + ".zip");
                                gameMenu.MenuAudit.HaveTheme = CheckForFile(fullPath);
                                break;
                            case "GenreBg":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Images.GenreBackgrounds);
                                gameMenu.MenuAudit.HaveGenreBG = CheckMediaFolderFiles(fullPath, "*.*");
                                break;
                            case "Pointer":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Images.Pointer);
                                gameMenu.MenuAudit.HavePointer = CheckMediaFolderFiles(fullPath, "*.*");
                                break;
                            case "GenreWheel":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Images.GenreWheel);
                                gameMenu.MenuAudit.HaveGenreWheel = CheckMediaFolderFiles(fullPath, "*.*");
                                break;
                            case "SystemStart":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Sound.SystemStart);
                                gameMenu.MenuAudit.HaveS_Start = CheckMediaFolderFiles(fullPath, "*.mp3");
                                break;
                            case "SystemExit":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, Sound.SystemExit);
                                gameMenu.MenuAudit.HaveS_Exit = CheckMediaFolderFiles(fullPath, "*.mp3");
                                break;
                            case "WheelClick":
                                tempPath = Path.Combine(_frontEnd.Path, HyperspinRootPaths.Media, gameMenu.RomName);
                                fullPath = Path.Combine(tempPath, "Sound", "Wheel Click.mp3");
                                gameMenu.MenuAudit.HaveWheelClick = CheckForFile(fullPath);
                                break;
                            default:
                                break;
                        }
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// Checks for video. Mp4 > FlV > PNG
        /// </summary>
        /// <param name="feSystemMediaPath">The frontendpath path.</param>
        /// <param name="gameName">Name of the game.</param>
        /// <returns></returns>
        private bool CheckForVideo(string feSystemMediaPath, string gameName)
        {
            //Video slightly different, where you have flvs & pngs
            var FullPath = Path.Combine(feSystemMediaPath, HyperspinRootPaths.Video, gameName + ".mp4");
            if (CheckForFile(FullPath)) return true;

            FullPath = Path.Combine(feSystemMediaPath, HyperspinRootPaths.Video, gameName + ".flv");
            if (CheckForFile(FullPath)) return true;

            FullPath = Path.Combine(feSystemMediaPath, HyperspinRootPaths.Video, gameName + ".png");
            if (CheckForFile(FullPath)) return true;

            return false;
        }

        #endregion

        #region Support Methods
        /// <summary>
        /// Check a given filename to exist
        /// </summary>
        /// <param name="filenamePath"></param>
        /// <returns></returns>
        private bool CheckForFile(string filenamePath)
        {
            if (File.Exists(filenamePath))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check all files in directory with given Extension
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="extFilter"></param>
        /// <returns></returns>
        private bool CheckMediaFolderFiles(string fullpath, string extFilter)
        {
            if (!Directory.Exists(fullpath))
                return false;

            string[] getFiles;
            getFiles = Directory.GetFiles(fullpath, extFilter);
            if (getFiles.Length != 0)
                return true;
            else return false;
        }
        #endregion
    }
}
