using Horsesoft.Frontends.Helper.Model.Hyperspin;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Tools
{
    public interface IMediaCopier
    {
        Task CopyMediaAsync(Game game, HsMediaType hsMediaType, string systemName, bool symbolicLink = false);
    }
}
