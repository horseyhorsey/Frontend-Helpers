using Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using Horsesoft.Frontends.Helper.Tools.Search;
using System.IO;
using System.Linq;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("HyperspinRealCollection")]
    public class HyperspinSearchTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinSearchTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        [Theory]
        [InlineData("Dizzy")]
        [InlineData("Robocop")]
        [InlineData("Kenny")]
        [InlineData("Commando")]
        [InlineData("Zero")]
        public async void SearchAmstradGamesFromXml__SearchResultsAreGreaterThan0(string search)
        {
            var sysName = "Amstrad CPC";
            var sysDbPath = PathHelper.GetSystemDatabasePath(_fixture._frontend.Path, sysName);
            var path = Path.Combine(sysDbPath, $"{sysName}.xml");

            ISearch<Game> searchXml = new XmlSearch<Game>();

            var foundGames = await searchXml.Search(sysName, path, new string[] { search });

            Assert.True(foundGames.Count() > 0);
        }

        [Theory]
        [InlineData("Dizzy", "Robocop")]
        [InlineData("Batman", "Danger Mouse")]
        public async void SearchAmstradMultipleGamesFromXml__SearchResultsAreGreaterThan0(string search1, string search2)
        {
            var sysName = "Amstrad CPC";
            var sysDbPath = PathHelper.GetSystemDatabasePath(_fixture._frontend.Path, sysName);
            var path = Path.Combine(sysDbPath, $"{sysName}.xml");

            ISearch<Game> searchXml = new XmlSearch<Game>();
            var foundGames = await searchXml.Search(sysName, path, new string[] { search1, search2 });

            Assert.True(foundGames.Count() > 0);
        }

        [Fact]
        public async void BuildGamesFromFavoritesText()
        {
            var sysName = "Amstrad CPC";
            var sysDbPath = PathHelper.GetSystemDatabasePath(_fixture._frontend.Path, sysName);
            var path = Path.Combine(sysDbPath, $"{sysName}.xml");

            _fixture._hyperSerializer.ChangeSystemAndDatabase(sysName);
            var amstradFaves = await _fixture._hyperSerializer.DeserializeFavoritesAsync();

            int faveCount = amstradFaves.Count();
            Assert.True(faveCount > 0);

            //Search the favorites list
            ISearch<Game> searchXml = new XmlSearch<Game>();            
            var foundFaves = await searchXml.Search(sysName, path, amstradFaves.Select(x=>x.RomName), true);
            
            Assert.True(foundFaves.Count() > (faveCount - 5));
        }
    }
}
