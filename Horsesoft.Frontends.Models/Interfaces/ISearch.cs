using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface ISearch<T>
    {
        Task<IEnumerable<T>> Search(string systemName, string xml, IEnumerable<string> searchList, bool exactMatch = false);
    }
}
