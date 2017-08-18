using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinAudit
    {
        /// <summary>
        /// Gets the unused media files asynchronous.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="hsMediaType">Type of the hs media.</param>
        /// <returns></returns>
        Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType hsMediaType);

        /// <summary>
        /// Scans for media asynchronous.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <returns>success</returns>
        Task<bool> ScanForMediaAsync(IEnumerable<Game> gamesList);

        /// <summary>
        /// Scans the main menu media asynchronous.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <returns>success</returns>
        Task<bool> ScanMainMenuMediaAsync(IEnumerable<Game> gamesList);
    }
}
