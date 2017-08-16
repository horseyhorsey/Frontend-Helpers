using Horsesoft.Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Models.RocketLauncher;
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