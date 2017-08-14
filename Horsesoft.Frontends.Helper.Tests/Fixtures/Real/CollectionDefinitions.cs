using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.Fixtures.Real
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinRealCollection : ICollectionFixture<HyperspinFixtureReal> { }

    [CollectionDefinition("RocketLaunchRealCollection")]
    public class RocketLaunchRealCollection : ICollectionFixture<RocketLaunchFixtureReal> { }
}
