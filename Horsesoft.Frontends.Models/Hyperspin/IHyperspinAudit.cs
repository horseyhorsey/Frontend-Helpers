using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinAudit
    {
        Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType hsMediaType);

        Task<bool> ScanForMediaAsync(IEnumerable<Game> gamesList);

        Task<bool> ScanMainMenuMediaAsync(IEnumerable<Game> gamesList);
    }
}
