using System.Collections.Generic;
using System.Threading.Tasks;
using Horsesoft.Frontends.Models.Hyperspin;
using System.IO;
using Hs.Hypermint.Services.Helpers;

namespace Horsesoft.Frontends.Helper.Tools
{
    /// <summary>
    /// Class for dealing with the games.ini in rocketlauncher
    /// </summary>
    /// <seealso cref="Horsesoft.Frontends.Helper.Tools.IRomMapperRl" />
    public class RomMapperRl : IRomMapperRl
    {
        /// <summary>
        /// Creates a rocketlauncher games ini asynchronously.</para>
        /// This is used mainly when creating multi systems
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="gamesIniPath">The games ini path.</param>
        /// <returns></returns>
        public async Task<bool> CreateGamesIniAsync(IEnumerable<Game> gamesList, string gamesIniPath)
        {
            return await Task.Run(() =>
            {
                bool result = false;

                var iniEndPath = new DirectoryInfo(gamesIniPath);
                var fi = new FileInfo(gamesIniPath + "\\games.ini");
                iniEndPath.Attributes &= FileAttributes.Normal;

                if (File.Exists(fi.FullName))
                    fi.Attributes &= ~FileAttributes.ReadOnly;

                var path = Path.Combine(gamesIniPath, "games.ini");

                using (StreamWriter file = new StreamWriter(path, false))
                {
                    file.WriteLine("# This file is only used for remapping specific games to other Emulators and/or Systems.");
                    file.WriteLine("# If you don't want your game to use the Default_Emulator, you would set the Emulator key here.");
                    file.WriteLine("# This file can also be used when you have Wheels with games from other Systems.");
                    file.WriteLine("# You would then use the System key to tell HyperLaunch what System to find the emulator settings.");
                    file.WriteLine("");
                    foreach (var game in gamesList)
                    {
                        file.WriteLine("[{0}]", game.RomName);
                        file.WriteLine(@"System={0}", game.System);
                    }
                }

                if (File.Exists(path))
                    result = true;

                return result;
            });
        }

        /// <summary>
        /// Gets the games from a rocket launch games ini asynchronously.
        /// </summary>
        /// <param name="gamesIniPath">The games ini path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Games.ini not found</exception>
        public async Task<Dictionary<string, string>> GetGamesFromRocketLaunchGamesIniAsync(string gamesIniPath)
        {
            if (!File.Exists(gamesIniPath))
                throw new FileNotFoundException("Games.ini not found");

            return await Task.Run(() =>
            {
                //load the ini file
                var ini = new IniFile();
                ini.Load(gamesIniPath);

                //Create a dictionary of game and system name
                var dict = new Dictionary<string, string>();
                foreach (var game in ini.Sections)
                {
                    dict.Add(game.ToString(), ini.GetKeyValue(game.ToString(), "System"));
                }

                return dict;
            });
        }
    }
}
