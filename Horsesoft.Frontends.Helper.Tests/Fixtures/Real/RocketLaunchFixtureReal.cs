using Horsesoft.Frontends.Helper.Auditing;
using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Serialization;
using Horsesoft.Frontends.Helper.Tools;
using System;

namespace Horsesoft.Frontends.Helper.Tests.Fixtures.Real
{
    public class RocketLaunchFixtureReal : FrontEndFixtureBase
    {
        public IFrontend _frontendRl;
        public IRocketLaunchAudit _auditer;
        public IHyperspinSerializer _hyperSerializer;
        public IRocketStats rlStats;

        public RocketLaunchFixtureReal()
        {
            //Create a hyperspin frontend based on this working directory
            _frontend = new Hyperspin.HyperspinFrontend()
            {
                Executable = "",
                HasSettingsFile = true,
                Path = Environment.CurrentDirectory + "\\Hyperspin Data"
            };

            _frontendRl = new Hyperspin.HyperspinFrontend()
            {
                Executable = "",
                HasSettingsFile = true,
                Path = Environment.CurrentDirectory + "\\RocketLauncherData",
                MediaPath = Environment.CurrentDirectory + "\\RocketLauncherData\\Media",
            };

            _auditer = new RocketLaunchAudit(_frontendRl);

            _hyperSerializer = new HyperspinSerializer(_frontend.Path, "", "");

            rlStats = new RocketStats(_frontendRl);
        }
    }
}
