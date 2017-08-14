using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Serialization
{
    public interface IDeserializer<T>
    {
        Task<IEnumerable<T>> DeserializeAsync();
    }
}
