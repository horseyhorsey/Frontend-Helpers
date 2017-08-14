using Horsesoft.Frontends.Helper.Model.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Tools
{
    public interface IRomScan
    {
        Task<bool> ScanRlRomPathsAsync(IEnumerable<Game> games, string rlPath, string systemName);
    }
}
