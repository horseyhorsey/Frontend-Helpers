using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Auditing;
using Horsesoft.Frontends.Helper.Media;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using System.Linq;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinAuditTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinAuditTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        /// <summary>
        /// Audits the hyperspin games list. 10th frame is first in the xml
        /// </summary>
        [Fact(DisplayName ="Audit Amstrad Scan for media, 10th frame artwork1 returns true")]
        public async void AuditHyperspinGamesList()
        {
            IHyperspinAudit auditer = new HyperspinAudit(frontend);

            _fixture._hyperSerializer.ChangeSystemAndDatabase("Amstrad CPC");
            var games = await _fixture._hyperSerializer.DeserializeAsync();            

            //Make sure scan fully completed
            Assert.True(await auditer.ScanForMediaAsync(games));

            //Check the mock data image was scanned
            Assert.True(games.ElementAt(0).MenuAudit.HaveArt1);
        }

        [Fact]
        public async void AuditMainMenusAsync()
        {
            IHyperspinAudit auditer = new HyperspinAudit(frontend);

            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu");
            var menus = await _fixture._hyperSerializer.DeserializeAsync();
            
            Assert.True(await auditer.ScanMainMenuMediaAsync(menus));

            Assert.True(menus.ElementAt(1).MenuAudit.HaveLetters);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveSpecial);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveWheel);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveVideo);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveGenreBG);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveGenreWheel);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveS_Start);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveS_Exit);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveWheelSounds);
            Assert.True(menus.ElementAt(1).MenuAudit.HavePointer);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveTheme);
            Assert.True(menus.ElementAt(1).MenuAudit.HaveWheelClick);
        }

        [Theory]
        [InlineData("Amstrad CPC", HsMediaType.Artwork1, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Artwork2, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Artwork3, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Artwork4, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Backgrounds, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Video, 1)]
        [InlineData("Amstrad CPC", HsMediaType.Wheel, 1)]
        public async void GetUnusedHyperspinMediaFiles(string systemName, HsMediaType mediaType, int expectedUnusedCount)
        {
            //Get some games to test  from
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            var games = await _fixture._hyperSerializer.DeserializeAsync();

            //Build an auditer with a mediahelper
            IMediaHelper mediaHelper = new MediaHelperHs(frontend.Path, systemName);
            IHyperspinAudit auditer = new HyperspinAudit(frontend, mediaHelper);

            var unusedFiles = await auditer.GetUnusedMediaFilesAsync(games, mediaType);

            //Make sure scan fully completed
            Assert.True(unusedFiles.Count() == expectedUnusedCount);
        }
    }
}
