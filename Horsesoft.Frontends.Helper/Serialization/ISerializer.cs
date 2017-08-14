using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Serialization
{
    public interface ISerializer<T>
    {
        Task<bool> SerializeAsync(IEnumerable<T> objectsToSerialize, bool isMultiSystem = false);
    }
}