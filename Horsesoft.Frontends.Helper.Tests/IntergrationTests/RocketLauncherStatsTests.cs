using System.Linq;
using Xunit;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("RocketLaunchRealCollection")]
    public class RocketLauncherStatsTests
    {
        public RocketLaunchFixtureReal _fixture;        

        public RocketLauncherStatsTests()
        {
            _fixture = new RocketLaunchFixtureReal();
        }

        [Theory(DisplayName = "Get Rocketlauncher single game stats")]
        [InlineData("Amstrad CPC","Dizzy - The Ultimate Cartoon Adventure (Europe)", 13)]
        [InlineData("Amstrad CPC","Fantasy World Dizzy (Europe)", 4)]
        [InlineData("Amstrad CPC","Shadow of the Beast (Europe) (Disk 1)", 11)]        
        public void GetRocketlaunchStatsForSingleAmstradGame(string system, string rom, int timesPlayed)
        {
            var game = new Game(rom, "");
            game.System = system;

            var stat = _fixture.rlStats.GetRlStats(game);

            Assert.NotNull(stat);

            Assert.True(stat.TimesPlayed == timesPlayed);
        }

        [Fact(DisplayName = "Get Amstrad Rocket Launch stats. Game Count is 111")]
        public async void GetRocketSystemStats()
        {
            var menu = new MainMenu("Amstrad CPC");

            var stats = (await _fixture.rlStats.GetRlStatsAsync(menu)).OrderBy(x=>x.Rom);            

            Assert.NotNull(stats);
            Assert.True(stats.Count() == 111);            
        }

        [Fact(DisplayName = "Get Main Menu Global Stats")]
        public async void GetRocketGlobalStats___AssertsCountOfValuesForEachOverZero()
        {
            var menu = new MainMenu("Main Menu");

            if (menu.Name.ToLower().Contains("main menu"))
            {
                var globalStats = await _fixture.rlStats.GetGlobalStatsAsync();
                
                Assert.NotNull(globalStats);

                Assert.True(globalStats.LastPlayedGames.Count > 0);
                Assert.True(globalStats.TopTenAverageTimePlayed.Count > 0);
                Assert.True(globalStats.TopTenSystemMostPlayed.Count > 0);
                Assert.True(globalStats.TopTenTimePlayed.Count > 0);
                Assert.True(globalStats.TopTenTimesPlayed.Count > 0);
            }
            else
                Assert.False(true);           
        }
    }
}
