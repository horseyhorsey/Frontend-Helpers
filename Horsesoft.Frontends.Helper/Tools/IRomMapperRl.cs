
using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Tools
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

        Task<Dictionary<string, string>> GetGamesFromRocketLaunchGamesIniAsync(string gamesIniPath);
    }
}
