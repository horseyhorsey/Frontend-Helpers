using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Systems;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using Horsesoft.Frontends.Helper.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [Collection("HyperspinRealCollection")]
    public class HyperspinMultiSystemTests
    {
        #region Fields
        public HyperspinFixtureReal _fixture;
        public IFrontend frontend;
        public ISystemCreator systemCreator;
        public IMediaCopier mediaCopier;
        public IEnumerable<Game> testGames; 
        #endregion

        public HyperspinMultiSystemTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
            systemCreator = new SystemCreator(frontend);
            mediaCopier = new MediaCopier(frontend);
            testGames = new List<Game>()
            {
                new Game("10th Frame (Europe)", "10th Frame"){ System = "Amstrad CPC", IsFavorite = true, Genre = "Bowling"},
                new Game("Dizzy", "Dizzy3"){ System = "Amstrad CPC", IsFavorite = false,Genre = "Adventure"},                             
                new Game("robocop", "Robocop"){ System = "MAME", IsFavorite = true,Genre = "Shoot em up"}
            };
        }

        [Theory]
        [InlineData("MultiSystem_1")]
        [InlineData("MultiSystem_2")]
        [InlineData("MultiSystem_3")]
        public async void CreateAHyperspinMultiSystem(string systemName, bool symbolic = false)
        {
            #region Setup
            var options = new MultiSystemOptions
            {
                MultiSystemName = systemName,
                CopyMedia = true,
                CreateSymbolicLinks = symbolic
            };

            //Create new seriliazer with multisystem path
            _fixture._hyperSerializer.ChangeSystemAndDatabase(options.MultiSystemName);
            IMultiSystem multiSystem = new MultiSystem(_fixture._hyperSerializer, systemCreator, mediaCopier, options);

            foreach (var game in testGames)
            {
                multiSystem.Add(game);
            }

            #endregion

            var result = await multiSystem.CreateMultiSystem(frontend.Path, "");

            Assert.True(result);
        }

        [Theory]
        [InlineData("MultiSystem_RomMapped")]
        public async void CreateAHyperspinMultiSystemWithRomMapAndGenresNoMedia(string systemName, bool symbolic = false)
        {
            #region Setup
            var options = new MultiSystemOptions
            {
                MultiSystemName = systemName,
                CopyMedia = false,
                CreateSymbolicLinks = false,
                CreateRomMap = true,
                CreateGenres = true
            };

            //Create new seriliazer with multisystem path
            _fixture._hyperSerializer.ChangeSystemAndDatabase(options.MultiSystemName);
            IMultiSystem multiSystem = new MultiSystem(_fixture._hyperSerializer, systemCreator, mediaCopier, options);

            foreach (var game in testGames)
            {
                multiSystem.Add(game);
            }

            #endregion

            var result = await multiSystem.CreateMultiSystem(frontend.Path, Environment.CurrentDirectory + "\\RocketLauncherData");
            Assert.True(result);

            Assert.True(File.Exists(Environment.CurrentDirectory + "\\RocketLauncherData\\Settings\\" + options.MultiSystemName + "\\games.ini"));
        }

        /// <summary>
        /// Creates a hyperspin multi system with symbolic links. Throws exception if not running as admin.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="symbolic">if set to <c>true</c> [symbolic].</param>
        [Theory(Skip ="Skip this because may fail on build server.")]
        [InlineData("MultiSystem_Symbolic")]
        public async void CreateAHyperspinMultiSystem_WithSymbolicLinks(string systemName, bool symbolic = true)
        {
            #region Setup
            var options = new MultiSystemOptions
            {
                MultiSystemName = systemName,
                CopyMedia = true,
                CreateSymbolicLinks = symbolic
            };

            //Create new seriliazer with multisystem path
            _fixture._hyperSerializer.ChangeSystemAndDatabase(options.MultiSystemName);
            IMultiSystem multiSystem = new MultiSystem(_fixture._hyperSerializer, systemCreator, mediaCopier, options);

            foreach (var game in testGames)
            {
                multiSystem.Add(game);
            }

            #endregion

            var result = await multiSystem.CreateMultiSystem(frontend.Path, "");

            Assert.True(result);
        }
    }
}
