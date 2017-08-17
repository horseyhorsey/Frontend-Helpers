using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface ISerializer<T>
    {
        Task<bool> SerializeAsync(IEnumerable<T> objectsToSerialize, bool isMultiSystem = false);
    }
}