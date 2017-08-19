using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class MediaCopier : IMediaCopier
    {
        private IFrontend _frontend;

        public MediaCopier(IFrontend frontend)
        {
            _frontend = frontend;
        }

        #region Public Methods

        /// <summary>
        /// Copies all media from all hyperspin types asynchronously.
        /// </summary>
        /// <param name="games">The games.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="symbolicLink">if set to <c>true</c> [symbolic link].</param>
        /// <returns></returns>
        public async Task CopyAllMediaAsync(IEnumerable<Game> games, string systemName, bool symbolicLink = false)
        {
            await Task.Run(async () =>
            {
                foreach (var game in games)
                {
                    foreach (HsMediaType mediaType in Enum.GetValues(typeof(HsMediaType)))
                    {
                        await CopyGameMediaAsync(game, mediaType, systemName, symbolicLink);
                    }
                }
            });
        }

        public async Task CopyGameMediaAsync(Game game, HsMediaType hsMediaType, string systemName, bool symbolicLink = false)
        {
            await Task.Run(() =>
            {
                var srcPath = PathHelper.GetMediaDirectoryForMediaType(_frontend.Path, game.System, hsMediaType);
                var destPath = PathHelper.GetMediaDirectoryForMediaType(_frontend.Path, systemName, hsMediaType);
                CopyMedia(game, srcPath, destPath, symbolicLink);
            });
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// Copies the media. TODO: THIS IS ONLY COPYING .PNG....
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="mediaPath">The media path.</param>
        /// <param name="symbolicLink">if set to <c>true</c> [symbolic link].</param>
        private void CopyMedia(Game game, string srcPath, string destPath, bool symbolicLink = false)
        {
            var fileToCopy = Path.Combine(srcPath, game.RomName + ".png");
            var fileDestination = Path.Combine(destPath, game.RomName + ".png");

            CopyFile(fileDestination, fileToCopy, symbolicLink);
        }

        /// <summary>
        /// Copies the file. Doesn't overwrite.
        /// </summary>
        /// <param name="fileDestination">The file destination.</param>
        /// <param name="fileToCopy">The file to copy.</param>
        /// <param name="symbolicLink">if set to <c>true</c> [symbolic link].</param>
        private void CopyFile(string fileDestination, string fileToCopy, bool symbolicLink)
        {
            if (!File.Exists(fileDestination))
            {
                if (File.Exists(fileToCopy))
                {
                    if (symbolicLink)
                    {
                        if (!SymbolicLinker.CheckThenCreate(fileToCopy, fileDestination))
                            throw new SymbolicLinkerException("Couldn't create a symbolic link, try running as administrator");
                    }                        
                    else
                        File.Copy(fileToCopy, fileDestination, true);
                }
            }
        }
        #endregion
    }
}
