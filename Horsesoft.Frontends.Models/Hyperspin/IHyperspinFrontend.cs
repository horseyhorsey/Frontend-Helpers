using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinFrontend
    {
        Task<IEnumerable<Game>> SearchXmlForGamesAsync(IEnumerable<string> gamesListToSearch, string systemName, IHyperspinSerializer serializer);

        Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer);

        /// <summary>
        /// Gets the databases that include the given system name in the frontend path.<para/>
        /// Also use for main menu xmls
        /// </summary>
        Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName);
    }
}
