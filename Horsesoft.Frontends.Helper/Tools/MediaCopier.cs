using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.IO;
using System.Threading.Tasks;

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
        public async Task CopyMediaAsync(Game game, HsMediaType hsMediaType, string systemName, bool symbolicLink = false)
        {
            await Task.Run(() =>
            {
                switch (hsMediaType)
                {
                    case HsMediaType.Artwork:
                        CopyArtworksForGame(game, systemName, symbolicLink);
                        break;
                    case HsMediaType.Backgrounds:
                        break;
                    case HsMediaType.Wheel:
                        break;
                    case HsMediaType.Video:
                        break;
                }
            });
        }
        #endregion

        #region Support Methods
        private void CopyArtworksForGame(Game game, string systemName, bool symbolicLink = false)
        {
            for (int i = 1; i < 5; i++)
            {
                var fileToCopy = Path.Combine(_frontend.Path, Root.Media, game.System, "Images\\Artwork" + i, game.RomName + ".png");
                var fileDestination = Path.Combine(_frontend.Path, Root.Media, systemName, "Images\\Artwork" + i, game.RomName + ".png");

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
        } 
        #endregion

        //private void CopyVideos(ref string hsPath, Game game)
        //{
        //    var FileToLink = Path.Combine(hsPath, Root.Media, game.System, Root.Video, game.RomName + ".mp4");
        //    var tempSymlinkFile = Path.Combine(hsPath, Root.Media, MultiSystemName, Root.Video, game.RomName + ".mp4");

        //    if (!File.Exists(tempSymlinkFile))
        //    {
        //        if (File.Exists(FileToLink))
        //        {
        //            SymbolicLinkService.CheckThenCreate(FileToLink, tempSymlinkFile);
        //            return;
        //        }
        //    }

        //    FileToLink = Path.Combine(hsPath, Root.Media, game.System, Root.Video, game.RomName + ".flv");
        //    tempSymlinkFile = Path.Combine(hsPath, Root.Media, MultiSystemName, Root.Video, game.RomName + ".flv");

        //    if (File.Exists(FileToLink))
        //    {
        //        if (CreateSymbolicLinks)
        //            SymbolicLinkService.CheckThenCreate(FileToLink, tempSymlinkFile);
        //        else
        //            File.Copy(FileToLink, tempSymlinkFile, true);
        //    }
        //}

        //private void CopyWheels(ref string hsPath, Game game)
        //{
        //    var FileToLink = Path.Combine(hsPath, Root.Media, game.System, Images.Wheels, game.RomName + ".png");
        //    var tempSymlinkFile = Path.Combine(hsPath, Root.Media, MultiSystemName, Images.Wheels, game.RomName + ".png");

        //    if (File.Exists(FileToLink))
        //    {
        //        if (CreateSymbolicLinks)
        //            SymbolicLinkService.CheckThenCreate(FileToLink, tempSymlinkFile);
        //        else
        //            File.Copy(FileToLink, tempSymlinkFile, true);
        //    }
        //}

        //private void CopyThemes(ref string hsPath, Game game)
        //{
        //    var FileToLink = Path.Combine(hsPath, Root.Media, game.System, Root.Themes, game.RomName + ".zip");
        //    var tempSymlinkFile = Path.Combine(hsPath, Root.Media, MultiSystemName, Root.Themes, game.RomName + ".zip");

        //    if (DefaultTheme)
        //    {
        //        if (!File.Exists(tempSymlinkFile))
        //        {
        //            if (!File.Exists(FileToLink))
        //                FileToLink = Path.Combine(hsPath, Root.Media, game.System, Root.Themes, "default.zip");
        //        }
        //    }

        //    if (CreateSymbolicLinks)
        //        SymbolicLinkService.CheckThenCreate(FileToLink, tempSymlinkFile);
        //    else
        //        File.Copy(FileToLink, tempSymlinkFile, true);
        //}
    }
}
