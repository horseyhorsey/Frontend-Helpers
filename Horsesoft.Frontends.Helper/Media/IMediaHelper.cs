using Horsesoft.Frontends.Helper.Model;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Media
{
    public interface IMediaHelper
    {
        /// <summary>
        /// Gets the unused media files.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <returns></returns>
        Task<IEnumerable<IFile>> GetUnusedMediaFiles(IEnumerable<Game> gamesList);
    }    
}
