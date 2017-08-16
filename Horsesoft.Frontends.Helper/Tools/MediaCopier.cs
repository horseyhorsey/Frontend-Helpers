using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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
                CopyMedia(game, PathHelper.GetMediaDirectoryForMediaType(_frontend.Path, systemName,hsMediaType), symbolicLink);
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
        private void CopyMedia(Game game, string mediaPath, bool symbolicLink = false)
        {
            var fileToCopy = Path.Combine(mediaPath, game.RomName + ".png");
            var fileDestination = Path.Combine(mediaPath, game.RomName + ".png");

            CopyFile(fileDestination, fileToCopy, symbolicLink);
        }

        private void CopyFile(string fileDestination, string fileToCopy, bool symbolicLink)
        {
            if (!File.Exists(fileDestination))
            {
                if (File.Exists(fileToCopy))
                {
                    if (symbolicLink)
                        SymbolicLinker.CheckThenCreate(fileToCopy, fileDestination);
                    else
                        File.Copy(fileToCopy, fileDestination, true);
                }
            }
        }
        #endregion
    }
}
