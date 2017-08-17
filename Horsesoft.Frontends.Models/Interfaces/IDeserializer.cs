using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface IDeserializer<T>
    {
        Task<IEnumerable<T>> DeserializeAsync();
    }
}
