using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Serialization;
using Horsesoft.Frontends.Helper.Settings;
using Horsesoft.Frontends.Helper.Systems;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using System.Collections.Generic;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("HyperspinRealCollection")]
    public class HyperspinMultiSystemTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinMultiSystemTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        [Fact(DisplayName = "Create A Hyperspin MultiSystem")]
        public async void CreateAHyperspinMultiSystem()
        {
            ISystemCreator systemCreator = new SystemCreator(frontend);
            IMediaCopier mediaCopier = new MediaCopier(frontend);

            //Create a games list from the Amstrad first
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Amstrad CPC");
            IEnumerable<Game> amstradGames = await _fixture._hyperSerializer.DeserializeAsync();

            //Create a serializer for new multi system
            var options = new MultiSystemOptions
            {
                MultiSystemName = "My Multi System"
            };

            //Create new seriliazer with multisystem path
            _fixture._hyperSerializer.ChangeSystemAndDatabase(options.MultiSystemName);

            IMultiSystem multiSystem = new MultiSystem(_fixture._hyperSerializer, systemCreator, mediaCopier, options);            
            foreach (var game in amstradGames)
            {
                multiSystem.Add(game);
            }

            var result = await multiSystem.CreateMultiSystem(frontend.Path);

            Assert.True(result);
        }
    }
}
