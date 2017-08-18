using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinFrontend
    {
        /// <summary>
        /// Gets the genre files for system asynchronous.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetGenreFilesForSystemAsync(string systemName, IHyperspinSerializer serializer);

        /// <summary>
        /// Gets the favorites asynchronous.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer);

        /// <summary>
        /// Gets the databases that include the given system name in the frontend path.<para/>
        /// Also use for main menu xmls
        /// </summary>
        Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName);

        /// <summary>
        /// Searches the XML for games asynchronous.
        /// </summary>
        /// <param name="gamesListToSearch">The games list to search.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> SearchXmlForGamesAsync(IEnumerable<string> gamesListToSearch, string systemName, IHyperspinSerializer serializer);
    }
}
