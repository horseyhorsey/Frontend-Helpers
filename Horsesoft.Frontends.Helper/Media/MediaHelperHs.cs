using Horsesoft.Frontends.Helper.Model;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
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
        public string CurrentFolder { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHelperHs"/> class and sets the CurrentFolder when created.
        /// </summary>
        /// <param name="fe">The fe.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="mediaFolder">The media folder.</param>
        public MediaHelperHs(string frontendPath, string systemName, string mediaType, string mediaFolder)
        {
            CurrentFolder = Path.Combine(frontendPath, Root.Media, systemName, mediaType, mediaFolder);
        }

        /// <summary>
        /// Gets the unused media files.
        /// </summary>
        /// <param name="gamesList">The games list.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public async Task<IEnumerable<IFile>> GetUnusedMediaFiles(IEnumerable<Game> gamesList)
        {
            if (!Directory.Exists(CurrentFolder))
                throw new DirectoryNotFoundException();

            return await Task.Run(() =>
            {
                var files = Directory.GetFiles(CurrentFolder);

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
