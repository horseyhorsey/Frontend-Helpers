using Horsesoft.Frontends.Helper.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher.Stats;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class RocketStats : IRocketStats
    {
        private string _rlPath;
        private readonly RocketStatsSerializer _rlStatSerializer;

        public RocketStats(string rlPath)
        {
            _rlPath = rlPath;
            _rlStatSerializer = new RocketStatsSerializer();
        }

        /// <summary>
        /// Gets the global stats.
        /// </summary>
        /// <returns></returns>
        public async Task<GlobalStats> GetGlobalStatsAsync()
        {
            return await _rlStatSerializer.GetGlobalStatsAsync(_rlPath);
        }

        /// <summary>
        /// Gets a single game stats by RomName
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public GameStat GetRlStats(Game game)
        {
            return _rlStatSerializer.GetSingleGameStats(_rlPath, game);
        }

        /// <summary>
        /// Gets all stats for this system
        /// </summary>
        /// <param name="mainMenu">The main menu.</param>
        /// <returns></returns>
        public async Task<IEnumerable<GameStat>> GetRlStatsAsync(MainMenu mainMenu)
        {
            return await _rlStatSerializer.GetStatsForSystemAsync(_rlPath, mainMenu);
        }
    }
}
