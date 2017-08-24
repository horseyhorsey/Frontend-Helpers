using System.Linq;
using Xunit;
using Horsesoft.Frontends.Helper.Tests.Fixtures.Real;
using System;
using System.Xml;
using Frontends.Models.Interfaces;
using Frontends.Models.Hyperspin;
using System.Collections.Generic;

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
        [InlineData("Amstrad CPC", "Amstrad CPC New", 1789)]
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

        [Theory]
        [InlineData("Amstrad CPC")]
        public async void SerializeGenres(string systemName)
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase(systemName);
            var games = await _fixture._hyperSerializer.DeserializeAsync();

            Assert.True(await _fixture._hyperSerializer.SerializeGenresAsync(games));
        }

        [Fact(DisplayName = "Deserialize Main Menu Items (Systems): Count should be 53")]
        public async void DeserializeHyperspinMainMenu()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu");

            var systems = await _fixture._hyperSerializer.DeserializeMenusAsync();

            Assert.True(systems.Count() == 53);
        }

        [Fact]
        public async void DeserializeHyperspinBadMainMenu__ElementNotClosed__ThrowsException()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu", "Main Menu Bad");

            var systems = await Assert.ThrowsAnyAsync<Exception>(async () => await _fixture._hyperSerializer.DeserializeMenusAsync());
        }

        [Fact]
        public async void DeserializeBad2MainMenu__ThrowsXmlExceptionBadFormatting()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu", "Main Menu Bad2");

            var systems = await Assert.ThrowsAnyAsync<XmlException>(async () => await _fixture._hyperSerializer.DeserializeMenusAsync());
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

        [Fact]
        public async void GetGamesListFromAllFavorites()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Main Menu");
            var systems = await _fixture._hyperSerializer.DeserializeMenusAsync();

            var favoriteGames = await _fixture._hyperSerializer.CreateGamesListFromAllFavoriteTexts(_fixture._frontend.Path, systems);

            Assert.True(favoriteGames.Count() > 0);
        }

        [Fact]
        public async void SerializeFavorites()
        {
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Amstrad CPC");

            var favoriteGames = await _fixture._hyperSerializer.DeserializeFavoritesAsync();
            _fixture._hyperSerializer.ChangeSystemAndDatabase("Dizzy Egg");

            var games = new List<Game>();
            foreach (var item in favoriteGames)
            {
                games.Add(new Game() { RomName = item.RomName, IsFavorite = true });
            }

            games.Add(new Game() { RomName = "nnsns", IsFavorite = true });

            Assert.True(await _fixture._hyperSerializer.SerializeFavoritesAsync(games));
        }
    }
}
