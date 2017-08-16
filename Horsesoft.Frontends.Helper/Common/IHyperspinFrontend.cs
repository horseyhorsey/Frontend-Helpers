using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Common
{
    public interface IHyperspinFrontend
    {
        Task<IEnumerable<Game>> SearchXmlForGamesAsync(IEnumerable<string> gamesListToSearch, string systemName, IHyperspinSerializer serializer);

        Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer);
    }
}
