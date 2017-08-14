using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Systems
{
    public interface ISystemCreator
    {
        Task<bool> CreateSystem(string systemName, bool existingDb = false);
    }
}
