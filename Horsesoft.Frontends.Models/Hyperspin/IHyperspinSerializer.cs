using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontends.Helper.Hyperspin
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

        Task<IEnumerable<MainMenu>> DeserializeMenusAsync();

        Task<IEnumerable<Favorite>> DeserializeFavoritesAsync();

        Task<IEnumerable<Genre>> DeserializeGenresAsync();

        Task<bool> SerializeFavoritesAsync(IEnumerable<Game> games);

        Task<bool> SerializeGenresAsync(IEnumerable<Game> games);
    }
}
