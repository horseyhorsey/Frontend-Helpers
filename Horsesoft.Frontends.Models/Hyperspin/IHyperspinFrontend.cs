using Frontends.Helper.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinFrontend
    {
        Task<IEnumerable<Game>> SearchXmlForGamesAsync(IEnumerable<string> gamesListToSearch, string systemName, IHyperspinSerializer serializer);

        Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer);

        Task<IEnumerable<string>> GetMainMenuDatabases();
    }
}
