using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using System.Linq;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinFrontendTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinFrontendTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        [Theory]
        [InlineData("Amstrad CPC", 5)]
        [InlineData("Atari ST", 0)]
        [InlineData("Main Menu", 2)]
        public async void GetDatabasesForSystem(string systemName, int expectedCount)
        {
            var databases = await frontend
                .GetDatabaseFilesForSystemAsync(systemName);            
            
            Assert.True(databases.Count() >= expectedCount);
        }


        [Theory]
        [InlineData("Amstrad CPC", 8)]
        public async void Genres__GetFiles(string systemName, int expected)
        {
            var hyperFrontend = (IHyperspinFrontend)(frontend);
            var databases = await hyperFrontend.GetGenreFilesForSystemAsync(systemName, _fixture._hyperSerializer);

            Assert.True(databases.Count() >= expected);
        }
    }
}
