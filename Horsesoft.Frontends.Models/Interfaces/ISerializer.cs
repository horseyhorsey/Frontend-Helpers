using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serializes asynchronous.
        /// </summary>
        /// <param name="objectsToSerialize">The objects to serialize.</param>
        /// <param name="isMultiSystem">if set to <c>true</c> [is multi system].</param>
        /// <returns></returns>
        Task<bool> SerializeAsync(IEnumerable<T> objectsToSerialize, bool isMultiSystem = false);
    }
}