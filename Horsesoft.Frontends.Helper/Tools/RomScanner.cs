using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Frontends.Models.Hyperspin;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class RomScanner : IRomScan
    {
        #region Public Methods
        /// <summary>
        /// Gets rom paths and extensions from a rocketlauncher system settings file and checks if the file exists.
        /// </summary>
        /// <param name="games">The games.</param>
        /// <param name="rlPath">The rl path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        public async Task<bool> ScanRlRomPathsAsync(IEnumerable<Game> games, string rlPath, string systemName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var emuName = RocketSettingsHelper.GetDefaultEmulator(rlPath, systemName);
                    var paths = RocketSettingsHelper.GetRomPaths(rlPath, systemName);
                    var exts = RocketSettingsHelper.GetRomExtensions(rlPath, emuName);

                    for (int i = 0; i < games.Count(); i++)
                    {
                        var game = games.ElementAt(i);

                        game.RomExists = RomExists(paths, exts, game.RomName);
                    }
                }
                catch { return false; }

                return true;
            });
        } 
        #endregion

        #region Support Methods

        /// <summary>
        /// Checks all paths and extension to see if a Rom exists.
        /// </summary>
        /// <param name="romPaths">The rom paths.</param>
        /// <param name="romExts">The rom exts.</param>
        /// <param name="romName">Name of the rom.</param>
        /// <returns></returns>
        private bool RomExists(string[] romPaths, string[] romExts, string romName)
        {
            for (int p = 0; p < romPaths.Length; p++)
            {
                for (int e = 0; e < romExts.Length; e++)
                {
                    if (File.Exists(romPaths[p] + "\\" + romName + "." + romExts[e]))
                        return true;
                }
            }

            return false;
        } 

        #endregion
    }
}
