using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Serialization;
using Horsesoft.Frontends.Helper.Settings;
using Horsesoft.Frontends.Helper.Systems;
using System;

namespace Horsesoft.Frontends.Helper.Tests.Fixtures.Real
{
    /// <summary>
    /// Fixture for hyperspin testing
    /// </summary>
    public class HyperspinFixtureReal : FrontEndFixtureBase
    {
        public IHyperspinSerializer _hyperSerializer;        
        public ISystemCreator _sysCreator;
        public ISettings _hsSettings;

        public HyperspinFixtureReal()
        {
            _frontend = new HyperspinFrontend()
            {
                Executable = "",
                HasSettingsFile = true,
                Path = Environment.CurrentDirectory + "\\Hyperspin Data"
            };

            _hyperSerializer = new HyperspinSerializer(_frontend.Path, "", "");

            _sysCreator = new SystemCreator(_frontend);

            _hsSettings = new HyperspinSettings(_frontend, "");
        }
    }
}
