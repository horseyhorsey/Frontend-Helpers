using System.Threading.Tasks;

namespace Frontends.Models.Interfaces
{
    public interface ISystemCreator
    {
        Task<bool> CreateSystem(string systemName, bool existingDb = false);
    }
}
