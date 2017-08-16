using Horsesoft.Frontends.Models;
using Horsesoft.Frontends.Models.Hyperspin;
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
        /// <param name="mediaFolder">The media folder.</param>
        /// <returns></returns>
        Task<IEnumerable<IFile>> GetUnusedMediaAsync(IEnumerable<Game> gamesList, string mediaFolder);

        /// <summary>
        /// Gets the unused media files.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType mediaType);        
    }    
}
