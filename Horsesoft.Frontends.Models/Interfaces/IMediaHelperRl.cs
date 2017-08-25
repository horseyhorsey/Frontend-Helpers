using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher;
using System.Collections.Generic;

namespace Frontends.Models.Interfaces
{
    public interface IMediaHelperRl
    {
        /// <summary>
        /// Gets all folders.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        string[] GetAllFolders(string dir);

        /// <summary>
        /// Matches the folders to games.
        /// </summary>
        /// <param name="directories">The directories.</param>
        /// <param name="gamesList">The games list.</param>
        /// <returns></returns>
        MediaScanResult MatchFoldersToGames(string[] directories, IEnumerable<Game> gamesList);
    }
}
