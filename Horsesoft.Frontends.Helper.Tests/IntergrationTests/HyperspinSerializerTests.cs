﻿using Horsesoft.Frontends.Helper.Common;
using System.Linq;
using Xunit;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;

namespace Horsesoft.Frontends.Helper.Tests.IntergrationTests
{
    [CollectionDefinition("HyperspinRealCollection")]
    public class HyperspinSerializerTests
    {
        public HyperspinFixtureReal _fixture;
        private IFrontend frontend;

        public HyperspinSerializerTests()
        {
            _fixture = new HyperspinFixtureReal();
            frontend = _fixture._frontend;
        }

        [Theory]
        [InlineData("Amstrad CPC", 1789)]
        [InlineData("Mame", 9)]
        [InlineData("Nintendo 64", 18)]
        public async void DeserilaizeGameXml(string systemName, int expectedCount)
        { 
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);

            var games = await _fixture._hyperSerializer.DeserializeAsync();

            Assert.True(games.Count() == expectedCount);
        }

        /// <summary>
        /// Seserializes to hyperspin XML. Gets the System names xml and creates a new one from that.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="newDbName">New name of the database.</param>
        /// <param name="expectedCount">The expected count.</param>
        [Theory]
        [InlineData("Amstrad CPC","Amstrad CPC New", 1789)]
        [InlineData("Mame", "Mame New", 9)]
        [InlineData("Nintendo 64", "Nintendo 64 New", 18)]
        public async void SerializeToHyperspinXml(string systemName, string newDbName, int expectedCount)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            var games = await _fixture._hyperSerializer.DeserializeAsync();

            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName, newDbName);
            Assert.True(await _fixture._hyperSerializer.SerializeAsync(games));
            
            games = await _fixture._hyperSerializer.DeserializeAsync();

            Assert.True(games.Count() == expectedCount);
        }

        [Fact(DisplayName = "Deserialize Main Menu Items (Systems): Count should be 53")]
        public async void DeserializeHyperspinMainMenu()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu");

            var systems = await _fixture._hyperSerializer.DeserializeMenusAsync();

            Assert.True(systems.Count() == 53);            
        }

        [Theory]
        [InlineData("Amstrad CPC", 25)]
        [InlineData("Atari ST", 17)]
        public async void DeserializeHyperspinFavorites(string systemName, int expectedCount)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);

            var faves = await _fixture._hyperSerializer.DeserializeFavoritesAsync();

            Assert.True(faves.Count() == expectedCount);
        }

        //[Theory]
        //[InlineData("Amstrad CPC")]
        //[InlineData("MAME")]
        //[InlineData("Nintendo 64")]
        //public async void SerializeHyperspinFavorites(string systemName)
        //{
        //    _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
        //    var faves = await _fixture._hyperSerializer.DeserializeFavoritesAsync();            

        //    Assert.True(await _fixture._hyperSerializer.SerializeFavoritesAsync(games));

        //    var txtPath = Path.Combine(frontend.Path, "Databases", systemName, "favorites.txt");
        //    Assert.True(File.Exists(txtPath));            
        //}
    }
}
