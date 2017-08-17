using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinSettingsTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinSettingsTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        /// <summary>
        /// Deserializes hyperspin system settings.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="romvalueIs">ExeInfo UseRomPath</param>
        [Theory]
        [InlineData("Amstrad CPC", true)]
        [InlineData("Atari ST", true)]
        public async void DeserializeHyperspinSettings(string systemName, bool romvalueIs)
        {                        
            Assert.True(await _fixture._hsSettings.DeserializeSettingsAsync(systemName));

            Assert.True(_fixture._hsSettings.Settings.ExeInfo.UseRomPath == romvalueIs);
        }
    }
}
