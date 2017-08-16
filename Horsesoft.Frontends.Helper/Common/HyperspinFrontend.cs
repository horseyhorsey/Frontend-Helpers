using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Hyperspin
{
    [Frontend(Models.FrontendType.Hyperspin)]
    public class HyperspinFrontend : FrontendBase, IHyperspinFrontend
    {
        public HyperspinFrontend()
        {
        }

        #region Public Methods
        public override bool Launch()
        {
            Console.WriteLine($"Launching {nameof(HyperspinFrontend)}...");

            return true;
        }

        public override Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName)
        {
            return Task.Run(() =>
            {
                if (systemName.Contains("Main Menu"))
                    throw new Exception("Can't get databases for a Main Menu");

                return GetDatabaseFilesForSystem(systemName, "*.xml");
            });
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesAsync(string systemName, IHyperspinSerializer serializer)
        {
            //Get favorites
            serializer.ChangeSystemAndDatabase(systemName);

            return await serializer.DeserializeFavoritesAsync();
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

                string mainMenuXml = System.IO.Path.Combine(Path, Root.Databases, systemName, $"{systemName}.xml");

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
