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
    public class RockeLauncherRomScanTests
    {
        public RocketLaunchFixtureReal _fixture;        
        private IEnumerable<Game> _games;

        public RockeLauncherRomScanTests()
        {
            _fixture = new Fixtures.Real.RocketLaunchFixtureReal();            
        }

        [Fact]        
        public async void ScanRomsTest()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Nintendo 64");
            var n64games = await _fixture._hyperSerializer.DeserializeAsync();
            Assert.True(n64games.Count() > 0);

            //Not really tested, needs roms
            IRomScan romScan = new RomScanner();
            await romScan.ScanRlRomPathsAsync(n64games, @"I:\RocketLauncher", "Nintendo 64");
        }
    }
}
