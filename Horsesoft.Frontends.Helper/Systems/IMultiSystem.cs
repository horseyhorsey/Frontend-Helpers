using Horsesoft.Frontends.Helper.Model.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Systems
{
    public interface IMultiSystem
    {
        bool Add(Game game);

        Task<bool> CreateMultiSystem(string frontEndPath);

        List<Game> Games { get; }

        bool Remove(Game game);
    }
}
