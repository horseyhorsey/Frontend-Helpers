using Horsesoft.Frontends.Helper.Auditing;
using Horsesoft.Frontends.Helper.Common;
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

        [Theory]
        [InlineData("Amstrad CPC", "Images", "Artwork1", 1)]
        public async void GetUnusedHyperspinMediaFiles(string systemName, string hsMediaFolder, string childMediaFolder, int expectedUnusedCount)
        {
            IMediaHelper mediaHelper = new MediaHelperHs(frontend.Path, systemName, hsMediaFolder, childMediaFolder);
            IHyperspinAudit auditer = new HyperspinAudit(frontend, mediaHelper);

            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            var games = await _fixture._hyperSerializer.DeserializeAsync();

            var unusedFiles = await auditer.GetUnusedMediaFilesAsync(games);

            //Make sure scan fully completed
            Assert.True(unusedFiles.Count() == expectedUnusedCount);
        }
    }
}
