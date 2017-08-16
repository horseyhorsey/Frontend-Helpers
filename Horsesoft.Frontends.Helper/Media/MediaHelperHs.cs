using Horsesoft.Frontends.Models;
using Horsesoft.Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Media
{
    /// <summary>
    /// Media helper for Hyperspin
    /// </summary>
    /// <seealso cref="Horsesoft.Frontends.Helper.Media.IMediaHelper" />
    public class MediaHelperHs : IMediaHelper
    {
        private string _frontendPath;
        private string _systemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHelperHs"/> class and sets the CurrentFolder when created.
        /// </summary>
        /// <param name="fe">The fe.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="mediaFolder">The media folder.</param>
        public MediaHelperHs(string frontendPath, string systemName)
        {
            _frontendPath = frontendPath;
            _systemName = systemName;
        }

        /// <summary>
        /// Gets the unused media files.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">
        /// </exception>
        public async Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType mediaType)
        {            
            //Join the path for this media type
            var fullPath = PathHelper.GetMediaDirectoryForMediaType(_frontendPath,_systemName, mediaType);

            if (!Directory.Exists(fullPath))
                throw new DirectoryNotFoundException(fullPath);

            return await GetUnusedMediaAsync(gamesList, fullPath);
        }

        /// <summary>
        /// Gets the unused media files from a full hyperspin media path
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <param name="mediaFolder">The media folder.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IFile>> GetUnusedMediaAsync(IEnumerable<Game> gamesList, string mediaFolder)
        {
            return await Task.Run(() =>
            {
                var files = Directory.GetFiles(mediaFolder);

                var hyperspinMediaFiles = new List<IFile>();

                foreach (var file in files)
                {
                    var fileName = file.ToLower();
                    if (fileName.Contains("thumbs.db") || !file.Contains("default.zip"))
                    {
                        var fileNoExt = Path.GetFileNameWithoutExtension(file);

                        if (!gamesList.Any(x => x.RomName == fileNoExt))
                        {
                            hyperspinMediaFiles.Add(new HyperspinFile(file));
                        }
                    }
                }

                return hyperspinMediaFiles.AsEnumerable();
            });
        }
    }
}
