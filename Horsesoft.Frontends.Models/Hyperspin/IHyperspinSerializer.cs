using Frontends.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Models.Hyperspin
{
    public interface IHyperspinSerializer : 
        IDeserializer<Game>, ISerializer<Game>, ISerializer<MainMenu>
    {
        /// <summary>
        /// Changes the system and database name.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="database">The name of the database.</param>
        void ChangeSystemAndDatabase(string systemName, string database = "");

        /// <summary>
        /// Deserializes the favorites asynchronous.
        /// </summary>
        Task<IEnumerable<Favorite>> DeserializeFavoritesAsync();

        /// <summary>
        /// Deserializes the genres asynchronous.
        /// </summary>
        Task<IEnumerable<Genre>> DeserializeGenresAsync();

        /// <summary>
        /// Deserializes the menus asynchronous.
        /// </summary>
        Task<IEnumerable<MainMenu>> DeserializeMenusAsync();

        /// <summary>
        /// Gets the genre files asynchronous.
        /// </summary>
        /// <param name="genreXmlPath">The genre XML path.</param>
        /// <returns></returns>
        Task<IEnumerable<Genre>> GetGenresAsync(string genreXmlPath);

        /// <summary>
        /// Serializes the favorites asynchronous.
        /// </summary>
        /// <param name="games">The games.</param>
        Task<bool> SerializeFavoritesAsync(IEnumerable<Game> games);

        /// <summary>
        /// Serializes the genres asynchronous.
        /// </summary>
        /// <param name="games">The games.</param>
        Task<bool> SerializeGenresAsync(IEnumerable<Game> games);
    }
}
