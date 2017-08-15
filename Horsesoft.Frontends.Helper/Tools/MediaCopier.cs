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
                switch (hsMediaType)
                {
                    case HsMediaType.Artwork:
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Artwork1), symbolicLink);
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Artwork2), symbolicLink);
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Artwork3), symbolicLink);
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Artwork4), symbolicLink);
                        break;
                    case HsMediaType.Backgrounds:
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Backgrounds), symbolicLink);
                        break;
                    case HsMediaType.Wheel:
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Images.Wheels), symbolicLink);
                        break;
                    case HsMediaType.Video:
                        CopyMedia(game, Path.Combine(_frontend.Path, "Media", systemName, Root.Video), symbolicLink);
                        break;
                }
            });
        }

        #endregion

        #region Support Methods

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
