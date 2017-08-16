using System.Collections.Generic;
using System.Threading.Tasks;
using Horsesoft.Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Models.RocketLauncher;
using Horsesoft.Frontends.Helper.Common;
using System.IO;
using System.Linq;
using Horsesoft.Frontends.Helper.Paths;
using System.Reflection;
using System;

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
        public async Task<bool> ScanAllSystemMedia(IEnumerable<Game> gamesList)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    foreach (var item in (typeof(RlMediaType).GetEnumValues()))
                    {
                        await ScanSystemMediaAsync((RlMediaType)item, gamesList);
                    }

                    return true;
                }
                catch
                { return false; }
            });
        }

        public async Task<bool> ScanSystemMediaAsync(RlMediaType rlMediaType, IEnumerable<Game> gamesList)
        {
            var systemName = gamesList.ElementAt(0).System;

            return await Task.Run(() =>
            {
                try
                {
                    switch (rlMediaType)
                    {
                        case RlMediaType.Artwork:
                            ScanRocketLauncherPath(gamesList, "HaveArtwork", RocketLauncherMediaPaths.Artwork, systemName);
                            break;
                        case RlMediaType.Backgrounds:
                            ScanRocketLauncherPath(gamesList, "HaveBackgrounds", RocketLauncherMediaPaths.Backgrounds, systemName);
                            break;
                        case RlMediaType.Bezels:
                            ScanRocketLauncherPath(gamesList, "HaveBezels", RocketLauncherMediaPaths.Bezels, systemName);
                            ScanRocketLauncherPath(gamesList, "HaveBezelBg", RocketLauncherMediaPaths.Bezels, systemName);
                            break;
                        case RlMediaType.Cards:
                            ScanRocketLauncherPath(gamesList, "HaveCards", RocketLauncherMediaPaths.Bezels, systemName);
                            break;
                        case RlMediaType.Controller:
                            ScanRocketLauncherPath(gamesList, "HaveController", RocketLauncherMediaPaths.Controller, systemName);
                            break;
                        case RlMediaType.Fade:
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer1", RocketLauncherMediaPaths.Fade, systemName);
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer2", RocketLauncherMediaPaths.Fade, systemName);
                            ScanRocketLauncherPath(gamesList, "HaveFadeLayer3", RocketLauncherMediaPaths.Fade, systemName);
                            ScanRocketLauncherPath(gamesList, "HaveExtraLayer1", RocketLauncherMediaPaths.Fade, systemName);
                            break;
                        case RlMediaType.Guides:
                            ScanRocketLauncherPath(gamesList, "HaveGuides", RocketLauncherMediaPaths.Guides, systemName);
                            break;
                        case RlMediaType.Manuals:
                            ScanRocketLauncherPath(gamesList, "HaveManuals", RocketLauncherMediaPaths.Manuals, systemName);
                            break;
                        case RlMediaType.MultiGame:
                            ScanRocketLauncherPath(gamesList, "HaveMultiGame", RocketLauncherMediaPaths.MultiGame, systemName);
                            break;
                        case RlMediaType.Music:
                            ScanRocketLauncherPath(gamesList, "HaveMusic", RocketLauncherMediaPaths.Music, systemName);
                            break;
                        case RlMediaType.SavedGames:
                            ScanRocketLauncherPath(gamesList, "HaveSavedGames", RocketLauncherMediaPaths.SavedGames, systemName);
                            break;
                        case RlMediaType.Settings:
                            break;
                        case RlMediaType.Videos:
                            ScanRocketLauncherPath(gamesList, "HaveVideos", RocketLauncherMediaPaths.Video, systemName);
                            break;
                        case RlMediaType.Wheels:
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
        /// <summary>
        /// Scans the rocket launcher path.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="propName">Name of the property.</param>
        /// <param name="rlPath">The rl path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <exception cref="DirectoryNotFoundException">Rocket media path not found.</exception>
        private void ScanRocketLauncherPath(IEnumerable<Game> gamesList, string propName, string rlPath, string systemName)
        {
            //Get current rlMedia system path for media type
            var scanPath = Path.Combine(_frontend.MediaPath, rlPath, systemName);

            if (!Directory.Exists(scanPath))
                throw new DirectoryNotFoundException("Rocket media path not found.");

            if (!Directory.Exists(scanPath)) return;

            foreach (var game in gamesList)
            {
                SetGameAudit(propName, scanPath, game);
            }
        }

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
            else
            {
                if (Directory.Exists(scanPath))
                    currProp.SetValue(game.RlAudit, true);
                else
                    currProp.SetValue(game.RlAudit, false);
            }
        }

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