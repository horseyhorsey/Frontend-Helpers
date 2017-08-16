using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher.Stats;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class RocketStats : IRocketStats
    {
        private string rlPath;
        private readonly RocketStatsSerializer _rlStatSerializer;

        public RocketStats(IFrontend frontend)
        {
            rlPath = frontend.Path;
            _rlStatSerializer = new RocketStatsSerializer();
        }

        /// <summary>
        /// Gets the global stats.
        /// </summary>
        /// <returns></returns>
        public async Task<GlobalStats> GetGlobalStatsAsync()
        {
            return await _rlStatSerializer.GetGlobalStatsAsync(rlPath);
        }

        /// <summary>
        /// Gets a single game stats by RomName
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public GameStat GetRlStats(Game game)
        {
            return _rlStatSerializer.GetSingleGameStats(rlPath, game);
        }

        /// <summary>
        /// Gets all stats for this system
        /// </summary>
        /// <param name="mainMenu">The main menu.</param>
        /// <returns></returns>
        public async Task<IEnumerable<GameStat>> GetRlStatsAsync(MainMenu mainMenu)
        {
            return await _rlStatSerializer.GetStatsForSystemAsync(rlPath, mainMenu);
        }
    }
}
