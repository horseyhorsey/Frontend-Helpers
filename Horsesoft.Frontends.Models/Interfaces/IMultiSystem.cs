using Frontends.Models.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IMultiSystem
    {
        bool Add(Game game);

        Task<bool> CreateMultiSystem(string frontEndPath, string rlPath);

        List<Game> Games { get; }

        bool Remove(Game game);
    }
}
