using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Horsesoft.Frontends.Helper.Paths;
using System.Reflection;
using Frontends.Models.RocketLauncher;
using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Auditing
{
    public class RocketLaunchAudit : IRocketLaunchAudit
    {
        private IFrontend _frontend;

        public RocketLaunchAudit(IFrontend frontend)
        {
            _frontend = frontend;
        }

        #region Public Methods
        /// <summary>
        /// Scans all system media asynchronous.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="rlPath">The rl path.</param>
        /// <returns></returns>
        public async Task<bool> ScanAllSystemMediaAsync(IEnumerable<Game> gamesList, string rlPath)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    foreach (var item in (typeof(RlMediaType).GetEnumValues()))
                    {
                        await ScanSystemMediaAsync((RlMediaType)item, gamesList, rlPath);
                    }

                    return true;
                }
                catch
                { return false; }
            });
        }

        /// <summary>
        /// Scans the default rlmedia folders for a system.
        /// </summary>
        /// <param name="rlPath">The rl path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        public async Task<RlAudit> ScanDefaultsForSystem(string rlPath, string systemName)
        {
            try
            {
                //Create a _default romname to scan
                var games = new List<Game>()
                    {
                        new Game("_Default", "_Default"){ System = systemName}
                    };

                //Scan each folder
                foreach (var item in (typeof(RlMediaType).GetEnumValues()))
                {
                    await ScanSystemMediaAsync((RlMediaType)item, games, rlPath);
                }

                //Return the audit
                return games[0].RlAudit;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Scans the system media asynchronous.
        /// </summary>
        /// <param name="rlMediaType">Type of the rl media.</param>
        /// <param name="gamesList">The games list.</param>
        /// <param name="rlMediaPath">The rl media path.</param>
        /// <returns></returns>
        public async Task<bool> ScanSystemMediaAsync(RlMediaType rlMediaType, IEnumerable<Game> gamesList, string rlMediaPath)
        {
            var systemName = gamesList.ElementAt(0).System;

            return await Task.Run(() =>
            {
                try
                {
                    switch (rlMediaType)
                    {
                        case RlMediaType.Artwork:
                            ScanRocketLauncherPath(gamesList, "HaveArtwork", RocketLauncherMediaPaths.Artwork, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveScreenshots", RocketLauncherMediaPaths.Artwork, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Backgrounds:
                            ScanRocketLauncherPath(gamesList, "HaveBackgrounds", RocketLauncherMediaPaths.Backgrounds, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Bezels:
                            ScanRocketLauncherPath(gamesList, "HaveBezels", RocketLauncherMediaPaths.Bezels, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveBezelBg", RocketLauncherMediaPaths.Bezels, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Cards:
                            ScanRocketLauncherPath(gamesList, "HaveCards", RocketLauncherMediaPaths.Bezels, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Controller:
                            ScanRocketLauncherPath(gamesList, "HaveController", RocketLauncherMediaPaths.Controller, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Fade:
                            ScanRocketLauncherPath(gamesList, "HaveFade", RocketLauncherMediaPaths.Fade, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer1", RocketLauncherMediaPaths.Fade, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer2", RocketLauncherMediaPaths.Fade, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer3", RocketLauncherMediaPaths.Fade, systemName, rlMediaPath);
                            ScanRocketLauncherPath(gamesList, "HaveExtraLayer1", RocketLauncherMediaPaths.Fade, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Guides:
                            ScanRocketLauncherPath(gamesList, "HaveGuide", RocketLauncherMediaPaths.Guides, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Logos:
                            ScanRocketLauncherPath(gamesList, "HaveLogos", RocketLauncherMediaPaths.Video, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Manuals:
                            ScanRocketLauncherPath(gamesList, "HaveManual", RocketLauncherMediaPaths.Manuals, systemName, rlMediaPath);
                            break;
                        case RlMediaType.MultiGame:
                            ScanRocketLauncherPath(gamesList, "HaveMultiGame", RocketLauncherMediaPaths.MultiGame, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Music:
                            ScanRocketLauncherPath(gamesList, "HaveMusic", RocketLauncherMediaPaths.Music, systemName, rlMediaPath);
                            break;
                        case RlMediaType.SavedGames:
                            ScanRocketLauncherPath(gamesList, "HaveSavedGames", RocketLauncherMediaPaths.SavedGames, systemName, rlMediaPath);
                            break;
                        case RlMediaType.Videos:
                            ScanRocketLauncherPath(gamesList, "HaveVideo", RocketLauncherMediaPaths.Video, systemName, rlMediaPath);
                            break;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
        #endregion

        #region Support Methods

        private bool isFadeColumn(string columnHeader)
        {
            switch (columnHeader)
            {
                case "Layer 1":
                case "Layer 2":
                case "Layer 3":
                case "Layer 4":
                case "Info Bar":
                case "Progress bar":
                case "7z Extracting":
                case "7z Complete":
                case "_Default Folder":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Scans the rocket launcher path.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="propName">Name of the property.</param>
        /// <param name="rlPath">The rl path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <exception cref="DirectoryNotFoundException">Rocket media path not found.</exception>
        private void ScanRocketLauncherPath(IEnumerable<Game> gamesList, string propName, string rlPath, string systemName, string rlMediaPath)
        {
            //Get current rlMedia system path for media type
            var scanPath = Path.Combine(rlMediaPath, rlPath, systemName);

            if (!Directory.Exists(scanPath))
                throw new DirectoryNotFoundException("Rocket media path not found.");

            if (!Directory.Exists(scanPath)) return;

            foreach (var game in gamesList)
            {
                SetGameAudit(propName, scanPath, game);
            }
        }

        /// <summary>
        /// Sets the game audit.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="scanPath">The scan path.</param>
        /// <param name="game">The game.</param>
        private void SetGameAudit(string propName, string scanPath, Game game)
        {
            scanPath = Path.Combine(scanPath, game.RomName);

            var rlAuditType = game.RlAudit.GetType();
            var currProp = rlAuditType.GetProperty(propName);

            //Deal with special cases or set the incoming property
            if (propName.Contains("Bezel"))
            {
                SetBezelAudit(propName, scanPath, game, currProp);
            }
            else if (propName.Contains("Layer"))
            {
                SetFadeAudit(propName, scanPath, game, currProp);
            }
            else if (propName == "HaveScreenshots")
            {
                if (Directory.Exists(scanPath + "\\Screenshots\\"))
                    currProp.SetValue(game.RlAudit, true);
                else
                    currProp.SetValue(game.RlAudit, false);
            }
            else
            {
                if (Directory.Exists(scanPath))
                    currProp.SetValue(game.RlAudit, true);
                else
                    currProp.SetValue(game.RlAudit, false);
            }
        }

        /// <summary>
        /// Sets the bezel audit. Bezels contain more than one media type. backgrounds, cards
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="scanPath">The scan path.</param>
        /// <param name="game">The game.</param>
        /// <param name="currProp">The curr property.</param>
        private void SetBezelAudit(string propName, string scanPath, Game game, PropertyInfo currProp)
        {
            string[] files = null;

            if (propName == "HaveBezels")
                files = Directory.GetFiles(scanPath, "Bezel*.*");
            else if (propName == "HaveBezelBg")
                files = Directory.GetFiles(scanPath, "Background*.*");
            else if (propName == "HaveCards")
                files = Directory.GetFiles(scanPath, "Instruction Card *.*");

            if (files.Length > 0)
                currProp.SetValue(game.RlAudit, true);
            else
                currProp.SetValue(game.RlAudit, false);
        }

        /// <summary>
        /// Sets the fade audit.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        /// <param name="scanPath">The scan path.</param>
        /// <param name="game">The game.</param>
        /// <param name="currProp">The curr property.</param>
        private void SetFadeAudit(string propName, string scanPath, Game game, PropertyInfo currProp)
        {
            string[] files = null;

            if (propName == "HaveFadeLayer1")
                files = Directory.GetFiles(scanPath, "Layer 1*.*");
            else if (propName == "HaveFadeLayer2")
                files = Directory.GetFiles(scanPath, "Layer 2*.*");
            else if (propName == "HaveFadeLayer3")
                files = Directory.GetFiles(scanPath, "Layer 3*.*");
            else if (propName == "HaveExtraLayer")
                files = Directory.GetFiles(scanPath, "Extra Layer 1*.*");
            else
                return;

            if (files.Length > 0)
                currProp.SetValue(game.RlAudit, true);
            else
                currProp.SetValue(game.RlAudit, false);
        }
        #endregion
    }
}