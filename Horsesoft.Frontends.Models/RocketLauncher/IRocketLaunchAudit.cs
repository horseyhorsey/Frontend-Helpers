using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.RocketLauncher
{
    public interface IRocketLaunchAudit
    {
        Task<bool> ScanAllSystemMedia(IEnumerable<Game> gamesList);

        Task<bool> ScanSystemMediaAsync(RlMediaType rlMediaType, IEnumerable<Game> gamesList);
    }
}