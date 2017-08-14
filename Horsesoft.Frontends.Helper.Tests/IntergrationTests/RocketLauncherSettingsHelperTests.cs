using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("RocketLaunchRealCollection")]
    public class RocketLauncherSettingsHelperTests
    {
        public RocketLaunchFixtureReal _fixture;

        public RocketLauncherSettingsHelperTests()
        {
            _fixture = new RocketLaunchFixtureReal();
        }

        [Fact]
        public void GetDefaultEmulatorForAmstrad__ShouldBeNamed__CPC()
        {
            var emulator = RocketSettingsHelper.GetDefaultEmulator(_fixture._frontendRl.Path, "Amstrad CPC");

            Assert.True(!string.IsNullOrWhiteSpace(emulator));

            Assert.True(emulator == "CPCE");
        }
    }
}
