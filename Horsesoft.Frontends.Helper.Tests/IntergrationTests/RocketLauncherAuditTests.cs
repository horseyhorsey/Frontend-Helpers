using System.Collections.Generic;
using System.Linq;
using Xunit;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher;

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

            Assert.True(await _fixture._auditer.ScanSystemMediaAsync(mediaType, _games, _fixture._frontendRl.MediaPath));

            Assert.True(_games.ElementAt(0).RlAudit.HaveArtwork);
        }

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void ScanAllRocketlauncherMedia(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            _games = await _fixture._hyperSerializer.DeserializeAsync();

            var result = await _fixture._auditer.ScanAllSystemMediaAsync(_games, _fixture._frontendRl.MediaPath);

            Assert.True(result, "Scan RL media failed");

            var game = _games.ElementAt(0);
            //assert we found the media for these
            Assert.True(game.RlAudit.HaveArtwork);
            Assert.True(game.RlAudit.HaveBackgrounds);
            Assert.True(game.RlAudit.HaveScreenshots);
        }

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void ScanAllDefaultFoldersRocketlauncherMedia(string systemName)
        {
            var defaultAudit = await _fixture._auditer.ScanDefaultsForSystem(_fixture._frontendRl.MediaPath, systemName);
            Assert.True(defaultAudit.HaveArtwork == true);
        }

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void ScanFadeMedia(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            _games = await _fixture._hyperSerializer.DeserializeAsync();

            var defaultAudit = await _fixture
                ._auditer.ScanSystemMediaAsync(RlMediaType.Fade, _games, _fixture._frontendRl.MediaPath);

            Assert.True(_games.ElementAt(0).RlAudit.HaveFade);
            Assert.True(_games.ElementAt(0).RlAudit.HaveFadeLayer1);
            Assert.True(_games.ElementAt(0).RlAudit.HaveFadeLayer2);
            Assert.True(_games.ElementAt(0).RlAudit.HaveFadeLayer3);
            Assert.True(_games.ElementAt(0).RlAudit.HaveExtraLayer1);
        }

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void ScanBezelMedia(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            _games = await _fixture._hyperSerializer.DeserializeAsync();

            await _fixture
                ._auditer.ScanSystemMediaAsync(RlMediaType.Bezels, _games, _fixture._frontendRl.MediaPath);

            await _fixture
                ._auditer.ScanSystemMediaAsync(RlMediaType.Cards, _games, _fixture._frontendRl.MediaPath);

            Assert.True(_games.ElementAt(0).RlAudit.HaveBezels);
            Assert.True(_games.ElementAt(0).RlAudit.HaveBezelBg);
            Assert.True(_games.ElementAt(0).RlAudit.HaveCards);
        }
    }
}
