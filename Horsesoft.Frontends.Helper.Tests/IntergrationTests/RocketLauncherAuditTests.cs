using System.Collections.Generic;
using System.Linq;
using Xunit;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("RocketLaunchRealCollection")]
    public class RocketLauncherAuditTests
    {
        public RocketLaunchFixtureReal _fixture;
        private IEnumerable<Game> _games;        

        public RocketLauncherAuditTests()
        {
            _fixture = new Fixtures.Real.RocketLaunchFixtureReal();
        }

        [Theory]
        [InlineData("Amstrad CPC", RlMediaType.Artwork)]
        public async void ScanRocketlauncherMedia(string systemName, RlMediaType mediaType)
        {                        
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);

            _games = await _fixture._hyperSerializer.DeserializeAsync();

            Assert.True(await _fixture._auditer.ScanSystemMediaAsync(mediaType, _games));

            Assert.True(_games.ElementAt(0).RlAudit.HaveArtwork);
        }

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void ScanAllRocketlauncherMedia(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            _games = await _fixture._hyperSerializer.DeserializeAsync();

            var result = await _fixture._auditer.ScanAllSystemMedia(_games);

            Assert.True(result, "Scan RL media failed");

            var game = _games.ElementAt(0);
            //assert we found the media for these
            Assert.True(game.RlAudit.HaveArtwork);
            Assert.True(game.RlAudit.HaveBackgrounds);
        }
    }
}
