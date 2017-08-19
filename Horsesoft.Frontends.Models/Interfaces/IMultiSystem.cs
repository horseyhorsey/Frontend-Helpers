using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IMultiSystem
    {
        Task<bool> CreateMultiSystem(IEnumerable<Game> games, string frontEndPath, string rlPath);
    }
}
