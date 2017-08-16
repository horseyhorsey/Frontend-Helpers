using Horsesoft.Frontends.Models.Hyperspin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Horsesoft.Frontends.Helper.Tools.Search
{
    public class XmlSearch<T> : ISearch<T>
    {
        /// <summary>
        /// Searches the specified system name.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="searchList">The search list.</param>
        /// <exception cref="XmlException"></exception>
        public Task<IEnumerable<T>> Search(string systemName, string xml, IEnumerable<string> searchList, bool exactMatch = false)
        {
            return Task.Run(() =>
            {
                //Set up reader , returning null games list.
                XDocument xdoc = null;
                using (var xmlreader = XmlReader.Create(xml))
                {
                    try { xdoc = XDocument.Load(xmlreader); }
                    catch { throw new XmlException(); }
                }

                //Build the games list from teh xml entries.
                var convertedGames = new List<Game>();
                foreach (var name in searchList)
                {
                    IEnumerable<Game> foundGames = null;
                    if (exactMatch)
                    {
                        foundGames = from item in xdoc.Descendants("game")
                                     where (item.Attribute("name").Value == name
                                           && (item.Element("cloneof").Value == ""))

                                     select new Game
                                     {
                                         RomName = item.Attribute("name").Value,
                                         GameEnabled = 1,
                                         Description = item.Element("description").Value,
                                         CloneOf = item.Element("cloneof").Value,
                                         Crc = item.Element("crc").Value,
                                         Manufacturer = item.Element("manufacturer").Value,
                                         Genre = item.Element("genre").Value,
                                         Year = Convert.ToInt32(item.Element("year").Value),
                                         Rating = item.Element("rating").Value,
                                         System = systemName
                                     };
                    }
                    else
                    {
                        foundGames = from item in xdoc.Descendants("game")
                                     where (item.Attribute("name").Value.Contains(name)
                                           && (item.Element("cloneof").Value == ""))

                                     select new Game
                                     {
                                         RomName = item.Attribute("name").Value,
                                         GameEnabled = 1,
                                         Description = item.Element("description").Value,
                                         CloneOf = item.Element("cloneof").Value,
                                         Crc = item.Element("crc").Value,
                                         Manufacturer = item.Element("manufacturer").Value,
                                         Genre = item.Element("genre").Value,
                                         Year = Convert.ToInt32(item.Element("year").Value),
                                         Rating = item.Element("rating").Value,
                                         System = systemName
                                     };
                    }                   
                    
                    foreach (Game game in foundGames)
                    {
                        //Create a games list from the found in the xml
                        Game fetchedGame = new Game();
                        if (game != null && !string.IsNullOrEmpty(game.RomName))
                        {
                            fetchedGame = new Game()
                            {
                                RomName = game.RomName,
                                Description = game.Description,
                                CloneOf = game.CloneOf,
                                Crc = game.Crc,
                                Manufacturer = game.Manufacturer,
                                Year = game.Year,
                                Genre = game.Genre,
                                Rating = game.Rating,
                                GameEnabled = game.GameEnabled,
                                System = game.System,
                                IsFavorite = true
                            };
                        }

                        if (string.IsNullOrEmpty(game.Description))
                            fetchedGame.Description = game.RomName;

                        convertedGames.Add(fetchedGame);
                    }                    
                }

                return (IEnumerable<T>)convertedGames.AsEnumerable();

            });
        }
    }
}
