
using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Tools
{
    public interface IMediaCopier
    {
        Task CopyAllMediaAsync(IEnumerable<Game> games, string systemName, bool symbolicLink = false);

        Task CopyGameMediaAsync(Game game, HsMediaType hsMediaType, string systemName, bool symbolicLink = false);
    }
}
