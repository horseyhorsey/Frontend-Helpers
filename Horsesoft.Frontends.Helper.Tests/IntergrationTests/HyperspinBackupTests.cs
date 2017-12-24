using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("HyperspinRealCollection")]
    public class HyperspinBackupTests
    {
        #region Setup Fixture

        #region Fields
        public HyperspinFixtureReal _fixture;
        public IFrontend frontend;
        #endregion

        public HyperspinBackupTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }
        #endregion

        #region Tests

        [Theory]
        [InlineData("AAE", Skip = "wefewf")]
        public async void CreateHyperspinBackUp(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            var games = await _fixture._hyperSerializer.DeserializeAsync();
            Assert.True(games.Count() > 0);

            //Backup all Databases for the system
            //BackupSystemDatabases(@"I:\HyperSpin", @"I:\HyperSpinBackUp", systemName);
            BackupSystemDatabases(@"I:\HyperSpin", @"\\Ponycade\d\from_horse\Hyperspin", systemName);

            //CreateDirectoryOnNetwork(@"\Ponycade\d\from_horse\test");
        }

        [Theory]
        [InlineData("AAE", Skip = "ewfew")]
        public async void CreateHyperspinBackUp_Video(string systemName)
        {
            //_fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            //var games = await _fixture._hyperSerializer.DeserializeAsync();
            //Assert.True(games.Count() > 0);

            BackupVideo(@"I:\HyperSpin", @"\\Ponycade\d\from_horse\Hyperspin", systemName);
        }

        [Theory]
        [InlineData("AAE", Skip = "wefefwef")]
        public async void CreateHyperspinBackUp_Wheels(string systemName)
        {
            //_fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            //var games = await _fixture._hyperSerializer.DeserializeAsync();
            //Assert.True(games.Count() > 0);

            BackupWheels(@"I:\HyperSpin", @"\\Ponycade\d\from_horse\Hyperspin", systemName);
        }

        [Theory]
        [InlineData("AAE", Skip = "wefefwfew")]
        public async void CreateHyperspinBackUp_Artwork(string systemName)
        {
            //_fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            //var games = await _fixture._hyperSerializer.DeserializeAsync();
            //Assert.True(games.Count() > 0);

            //BackupArtwork(@"I:\HyperSpin", @"\\Ponycade\d\from_horse\Hyperspin", systemName);
        }

        [Fact(Skip = "wwiwjdwd")]
        public void BackUpRomsForSystem()
        {

        }

        [Fact(Skip = "wefewfwefwe")]
        public void GetTotalSizeOfHyperspinMedia()
        {
            var directoryInfos = new List<string>();

            DirectoryInfo di = new DirectoryInfo(@"I:\HyperSpin\Media\");

            long totalSize = 0;
            long systemSize = 0;

            foreach (var directory in di.EnumerateDirectories())
            {
                systemSize = 0;
                var newInfoName = directory.FullName;

                systemSize += GetSizeOfDirectory(newInfoName);

                directoryInfos.Add(newInfoName + "," + systemSize);

                totalSize += systemSize;
            }

            directoryInfos.Add("--------------------------");
            directoryInfos.Add($"Total Media Size: {totalSize}");
        }

        [Fact(Skip = "Don't be lazy....and come back please.")]
        public void BackUpSystemsMedia()
        {
            HyperspinBackup hsb = new HyperspinBackup(@"I:\\Hyperspin", "Amstrad CPC");

            hsb.BackupMediaFolder(@"C:\zz_HyperspinBackup", false);
        }

        [Fact(Skip = "fewweffewefwfe")]
        public void RocketLaunchRomPathSize()
        {
            var romPaths = RocketSettingsHelper.GetRomPaths(@"I:\\RocketLauncher", "Amstrad CPC");

            Assert.True(romPaths.Count() > 0);

            var romDirSize = GetSizeOfDirectory(romPaths[0]);
        }

        private long GetSizeOfDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            var fileInfos = di.GetFileSystemInfos("*.*", SearchOption.AllDirectories);
            
            long dirSize = 0;
            foreach (FileInfo file in fileInfos.Where(x => x.Attributes == FileAttributes.Archive))
            {
                dirSize += file.Length;
            }

            return dirSize;
        }

        private void BackupArtwork(string hyperspinPath, string destPath, string systemName, bool overwrite = false)
        {
            var artPath = (HyperspinPaths.GetMediaDirectory(hyperspinPath, systemName, HsMediaType.Artwork1));
            var destEndPath = HyperspinPaths.GetMediaDirectory(destPath, systemName, HsMediaType.Artwork1);

            var newArtPath = artPath.Substring(0, artPath.Length - 1);
            var newdestEndPath = destEndPath.Substring(0, destEndPath.Length - 1);        
                           
            for (int i = 1; i < 5; i++)
            {
                BackupFiles(newArtPath + i, newdestEndPath + i, overwrite);
            }            
        }

        #endregion


        #region Support Methods

        private void BackupVideo(string hyperspinPath, string destPath, string systemName, bool overwrite = false)
        {
            var videoPath = HyperspinPaths.GetMediaDirectory(hyperspinPath, systemName, HsMediaType.Video);
            var destEndPath = Path.Combine(destPath, HyperspinRootPaths.Media, systemName, HyperspinRootPaths.Video);

            BackupFiles(videoPath, destEndPath, overwrite);
        }

        private void BackupWheels(string hyperspinPath, string destPath, string systemName, bool overwrite = false)
        {
            //Get the wheel path
            var wheelPath = HyperspinPaths.GetMediaDirectory(hyperspinPath, systemName, HsMediaType.Wheel);

            //Create a wheel path based on existing hyperspin installs
            var destEndPath = Path.Combine(destPath, HyperspinRootPaths.Media, systemName, Images.Wheels);

            BackupFiles(wheelPath, destEndPath, overwrite);
        }

        /// <summary>
        /// Backups the system database folder to the destination path as dest + "Hyperspin\Databases\SystemName"
        /// </summary>
        /// <param name="hyperspinPath">The hyperspin path.</param>
        /// <param name="destPath">The dest path.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        private void BackupSystemDatabases(string hyperspinPath, string destPath, string systemName, bool overwrite = false)
        {
            var dbPath = HyperspinPaths.GetSystemDatabasePath(hyperspinPath, systemName);
            var destDbPath = HyperspinPaths.GetSystemDatabasePath(destPath, systemName);

            BackupFiles(dbPath, destDbPath, overwrite);
        }

        /// <summary>
        /// Backups all the files from Source to Destination
        /// </summary>
        /// <param name="srcPath">The source path.</param>
        /// <param name="dstPath">The DST path.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        private void BackupFiles(string srcPath, string dstPath, bool overwrite)
        {
            var files = Directory.EnumerateFiles(srcPath);
            CopyFiles(files, dstPath, overwrite);
        }

        /// <summary>
        /// Copies a list of files to a given directory
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="destPath">The dest path.</param>
        /// <param name="overwrite">If file exists overwrite?</param>
        private void CopyFiles(IEnumerable<string> files, string destPath, bool overwrite)
        {
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            foreach (var file in files)
            {
                var destFilename = Path.GetFileName(file);
                var finalPath = Path.Combine(destPath, destFilename);
                var destFileExists = File.Exists(finalPath);

                //Overwrite existing
                if (destFileExists && overwrite)
                    File.Copy(file, finalPath, true);
                //Create new file
                else if (!destFileExists)
                    File.Copy(file, finalPath);
            }
        }

        #endregion

    }
}
