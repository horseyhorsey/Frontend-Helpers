using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IRomMapperRl
    {
        /// <summary>
        /// Creates a rocketlauncher games ini asynchronously.</para>
        /// This is used mainly when creating multi systems
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="gamesIniPath">The games ini path.</param>
        /// <returns></returns>
        Task<bool> CreateGamesIniAsync(IEnumerable<Game> gamesList, string gamesIniPath);

        Task<IEnumerable<Game>> GetGamesFromRocketLaunchGamesIniAsync(string rlSystemSettingPath);
    }
}
