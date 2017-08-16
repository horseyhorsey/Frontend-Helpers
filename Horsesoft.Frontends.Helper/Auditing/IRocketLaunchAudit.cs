using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Auditing
{
    public interface IRocketLaunchAudit
    {
        Task<bool> ScanAllSystemMedia(IEnumerable<Game> gamesList);

        Task<bool> ScanSystemMediaAsync(RlMediaType rlMediaType, IEnumerable<Game> gamesList);
    }
}