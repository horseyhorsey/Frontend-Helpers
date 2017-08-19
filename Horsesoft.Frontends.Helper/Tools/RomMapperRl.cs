using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Hs.Hypermint.Services.Helpers;
using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;

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

                if (!Directory.Exists(gamesIniPath))
                    Directory.CreateDirectory(gamesIniPath);

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
        /// <param name="rlSystemSettingPath">The games ini path.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">Games.ini not found</exception>
        public async Task<IEnumerable<Game>> GetGamesFromRocketLaunchGamesIniAsync(string rlSystemSettingPath)
        {
            var iniPath = rlSystemSettingPath + "\\games.ini";
            if (!File.Exists(iniPath))
                throw new FileNotFoundException("Games.ini not found");

            return await Task.Run(() =>
            {
                List<Game> games = new List<Game>();

                try
                {
                //load the ini file
                var ini = new IniFile();
                    ini.Load(iniPath);

                    foreach (IniFile.IniSection game in ini.Sections)
                    {
                        var romname = game.Name;
                        var system = ini.GetKeyValue(romname, "System");
                        var iniGame = new Game(romname, romname) { System = system };
                        games.Add(iniGame);
                    }

                    return games;
                }
                catch (System.Exception) { throw; }
            });
        }
    }
}
