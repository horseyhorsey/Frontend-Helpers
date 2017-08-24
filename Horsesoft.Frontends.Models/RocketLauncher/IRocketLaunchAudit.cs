using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.RocketLauncher
{
    public interface IRocketLaunchAudit
    {
        Task<RlAudit> ScanDefaultsForSystem(string rlPath, string systemName);

        Task<bool> ScanAllSystemMediaAsync(IEnumerable<Game> gamesList, string rlPath);

        Task<bool> ScanSystemMediaAsync(RlMediaType rlMediaType, IEnumerable<Game> gamesList, string rlPath);
    }
}