using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher.Stats;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IRocketStats
    {
        /// <summary>
        /// Gets a single game stats by RomName
        /// </summary>
        /// <param name="game">The game.</param>
        GameStat GetRlStats(Game game);

        /// <summary>
        /// Gets all stats for this system
        /// </summary>
        /// <param name="mainMenu">The main menu.</param>
        /// <returns></returns>
        Task<IEnumerable<GameStat>> GetRlStatsAsync(MainMenu mainMenu);

        /// <summary>
        /// Gets the global stats.
        /// </summary>
        /// <returns></returns>
        Task<GlobalStats> GetGlobalStatsAsync();
    }
}
