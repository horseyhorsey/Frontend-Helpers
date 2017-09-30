using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    public class HyperspinPathHelperTests
    {
        [Fact]
        public void GetMediaFilesForGame()
        {
            var hsPath = Environment.CurrentDirectory + "\\Hyperspin Data\\";

            var files = Paths.Hyperspin.HyperspinPaths.GetMediaFilesForGame(hsPath, "Amstrad CPC", Images.Wheels, "1943*.*");
        }
    }
}
