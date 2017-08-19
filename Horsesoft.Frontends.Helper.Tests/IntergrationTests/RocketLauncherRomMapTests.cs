using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("RocketLaunchRealCollection")]
    public class RocketLauncherRomMapTests
    {
        #region Constructors / fields
        public RocketLaunchFixtureReal _fixture;
        public IRomMapperRl romMapper;
        private List<Game> testGames;

        public RocketLauncherRomMapTests()
        {
            _fixture = new RocketLaunchFixtureReal();
            romMapper = new RomMapperRl();

            testGames = new List<Game>()
            {
                new Game("10th Frame (Europe)", "10th Frame"){ System = "Amstrad CPC"},
                new Game("Dizzy", "Dizzy3"){ System = "Amstrad CPC"},
                new Game("robocop", "Robocop"){ System = "MAME"}
            };

        } 
        #endregion

        [Fact]
        public async void CreateRomMapFromGamesAsync()
        {
            Assert.True(await romMapper.CreateGamesIniAsync(testGames, _fixture._frontendRl.Path));
        }


        [Fact]
        public async void GetGamesIniToGames_Assert10thFrameExists()
        {            
           var games = await romMapper
                .GetGamesFromRocketLaunchGamesIniAsync(_fixture._frontendRl.Path);

            Assert.True(games.Count() > 0);
            Assert.True(games.Any(x=> x.RomName == "10th Frame (Europe)"));
        }

        [Fact]
        public async void GetGamesIniToGames_AssertRobocopsSystemIsMame()
        {
            var games = await romMapper
                 .GetGamesFromRocketLaunchGamesIniAsync(_fixture._frontendRl.Path);

            Assert.True(games.Count() > 0);
            Assert.True(games.Any(x => x.RomName == "robocop" && 
            x.System == "MAME"));
        }
    }
}
