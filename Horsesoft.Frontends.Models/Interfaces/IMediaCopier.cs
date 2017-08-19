using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IMediaCopier
    {
        Task CopyAllMediaAsync(IEnumerable<Game> games, string systemName, bool symbolicLink = false);

        Task CopyGameMediaAsync(Game game, HsMediaType hsMediaType, string systemName, bool symbolicLink = false);
    }
}
