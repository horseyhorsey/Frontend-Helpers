using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinCreateSystemTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinCreateSystemTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        [Theory]
        [InlineData("Amstrad CPC New System")]
        [InlineData("Amiga 7000")]
        [InlineData("Dizzy Collection")]
        [InlineData("Cybernoid Hits ȡȡȡ")]
        public async void CreateAHyperspinSystem(string systemNameToCreate)
        {
            var result = await _fixture._sysCreator.CreateSystem(systemNameToCreate);

            Assert.True(result);
        }
    }
}