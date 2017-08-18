using Frontends.Models;
using Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Common.Attributes;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Horsesoft.Frontends.Helper.Common
{
    [Frontend(FrontendType.Hyperspin)]
    public class HyperspinFrontend : FrontendBase, IHyperspinFrontend
    {
        public HyperspinFrontend()
        {
        }

        #region Public Methods        

        /// <summary>
        /// Gets the database files for system asynchronous. Includes main menu items
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        public override Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName)
        {
            return Task.Run(() =>
            {
                return GetDatabaseFilesForSystem(systemName, "*.xml");
            });
        }

        /// <summary>
        /// Gets the genre files for system asynchronous.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Genre>> GetGenreFilesForSystemAsync(string systemName, IHyperspinSerializer serializer)
        {
            serializer.ChangeSystemAndDatabase(systemName);

            var path = PathHelper.GetSystemDatabasePath(Path, systemName);

            return await serializer.GetGenresAsync(path + "\\genre.xml");
        }

        /// <summary>
        /// Gets the favorites asynchronous.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        public async Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer)
        {
            //Get favorites
            serializer.ChangeSystemAndDatabase(systemName);

            return await serializer.DeserializeFavoritesAsync();
        }

        /// <summary>
        /// Launch the frontend
        /// </summary>
        /// <returns></returns>
        public override bool Launch()
        {
            Console.WriteLine($"Launching {nameof(HyperspinFrontend)}...");

            return true;
        }

        /// <summary>
        /// Searches a system XML from a games list asynchronously.
        /// </summary>
        /// <param name="gamesListToSearch">The games list to search.</param>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Games list count cannot be zero</exception>
        public async Task<IEnumerable<Game>> SearchXmlForGamesAsync(IEnumerable<string> gamesListToSearch, string systemName, IHyperspinSerializer serializer)
        {
            if (gamesListToSearch.Count() == 0) throw new Exception("Games list count cannot be zero");

            return await Task.Run(() =>
            {
                var fetchedGames = new List<Game>();

                string mainMenuXml = System.IO.Path.Combine(PathHelper.GetSystemDatabasePath(Path, systemName), $"{systemName}.xml");

                //Set up reader , returning null games list.
                XDocument xdoc = null;
                using (var xmlreader = XmlReader.Create(mainMenuXml))
                {
                    try { xdoc = XDocument.Load(xmlreader); }
                    catch (XmlException) { throw; }
                }

                try
                {
                    //Go over favorites and match a game from within the xml
                    foreach (var romName in gamesListToSearch)
                    {
                        var gameVars = from item in xdoc.Descendants("game")
                                       where (item.Attribute("name").Value.Contains(romName)
                                             && (item.Element("cloneof").Value == ""))

                                       select new
                                       {
                                           RomName = item.Attribute("name").Value,
                                           Enabled = 1,
                                           Description = item.Element("description").Value,
                                           CloneOf = item.Element("cloneof").Value,
                                           CRC = item.Element("crc").Value,
                                           Manufacturer = item.Element("manufacturer").Value,
                                           Genre = item.Element("genre").Value,
                                           Year = Convert.ToInt32(item.Element("year").Value),
                                           Rating = item.Element("rating").Value,
                                           System = systemName
                                       };

                        try
                        {
                            //Create a games list from the found in the xml
                            Game fetchedGame = new Game();
                            foreach (var game in gameVars)
                            {
                                if (game != null && !string.IsNullOrEmpty(game.RomName))
                                {
                                    fetchedGame = new Game()
                                    {
                                        RomName = game.RomName,
                                        Description = game.Description,
                                        CloneOf = game.CloneOf,
                                        Crc = game.CRC,
                                        Manufacturer = game.Manufacturer,
                                        Year = game.Year,
                                        Genre = game.Genre,
                                        Rating = game.Rating,
                                        GameEnabled = game.Enabled,
                                        System = game.System,
                                        IsFavorite = true
                                    };
                                }

                                if (string.IsNullOrEmpty(game.Description))
                                    fetchedGame.Description = game.RomName;

                                fetchedGames.Add(fetchedGame);
                            }

                        }
                        catch { }
                    }
                }
                catch { }

                return fetchedGames.AsEnumerable();
            });
        }

        #endregion
    }
}
