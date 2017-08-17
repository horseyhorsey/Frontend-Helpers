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
                        tempPath = Path.Combine(hsPath, Root.Media, systemName);

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

                        FullPath = Path.Combine(tempPath, Sound.SystemStart);
                        currentGameAudit.MenuAudit.HaveS_Start = CheckMediaFolderFiles(FullPath, "*.mp3");

                        FullPath = Path.Combine(tempPath, Sound.SystemExit);
                        currentGameAudit.MenuAudit.HaveS_Exit = CheckMediaFolderFiles(FullPath, "*.mp3");

                        FullPath = Path.Combine(tempPath, Root.Themes, currentGameAudit.RomName + ".zip");
                        currentGameAudit.MenuAudit.HaveTheme = CheckForFile(FullPath);

                        //Video slightly different, where you have flvs & pngs
                        FullPath = Path.Combine(tempPath, Root.Video, currentGameAudit.RomName + ".mp4");
                        if (!CheckForFile(FullPath))
                            FullPath = Path.Combine(tempPath, Root.Video, currentGameAudit.RomName + ".flv");
                        if (!CheckForFile(FullPath))
                            FullPath = Path.Combine(tempPath, Root.Video, currentGameAudit.RomName + ".png");
                        if (!CheckForFile(FullPath))
                            currentGameAudit.MenuAudit.HaveVideo = false;
                        else
                            currentGameAudit.MenuAudit.HaveVideo = true;
                    }
                }

                return true;
            });
        }

        public Task<bool> ScanMainMenuMediaAsync(IEnumerable<Game> gamesList)
        {
            throw new NotImplementedException();
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
