using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IRomScan
    {
        Task<bool> ScanRlRomPathsAsync(IEnumerable<Game> games, string rlPath, string systemName);
    }
}
