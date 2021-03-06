﻿using Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Tools.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Horsesoft.Frontends.Helper.Serialization
{
    public class HyperspinSerializer : IHyperspinSerializer
    {
        #region Fields
        private string _systemName;
        private string _frontEndPath;
        private string _databaseName;
        #endregion

        #region Constructor
        public HyperspinSerializer(string frontEndPath, string systemName, string databaseName = null)
        {
            _frontEndPath = frontEndPath;

            ChangeSystemAndDatabase(systemName, databaseName);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the system and database name. If database is null or empty the system name will be used for the database name
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="database">The name of the database.</param>
        public void ChangeSystemAndDatabase(string systemName, string database = "")
        {
            _systemName = systemName;

            _systemName = systemName;
            _databaseName = database;

            if (string.IsNullOrWhiteSpace(_databaseName))
                _databaseName = _systemName;
        }

        /// <summary>
        /// Creates a games list from all favorite texts from all given systems.
        /// </summary>
        /// <param name="frontEndPath">The front end path.</param>
        /// <param name="systems">The systems.</param>
        /// <returns></returns>
        public Task<IEnumerable<Game>> CreateGamesListFromAllFavoriteTexts(string frontEndPath, IEnumerable<MainMenu> systems)
        {
            return Task.Run(async () =>
            {
                if (!Directory.Exists(frontEndPath))
                    throw new DirectoryNotFoundException($"Frontend path {frontEndPath } doesn't exist.");

                IEnumerable<Game> games = null;

                try
                {
                    games = await ScanFavorites(frontEndPath, systems);                    
                }
                //Skip file not found because
                catch (FileNotFoundException){}
                catch (Exception)
                {
                    throw;
                }

                return games;
            });
        }

        /// <summary>
        /// Deserializes hyperspin games asynchronously from a database xml
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Game>> DeserializeAsync()
        {
            return await Task.Run<IEnumerable<Game>>(() =>
            {
                var systemDbPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName);
                var dbPath = Path.Combine(systemDbPath, _databaseName + ".xml");

                if (!Directory.Exists(systemDbPath))
                    throw new FileNotFoundException($"Couldn't find a database xml for system: {_systemName} @ {systemDbPath}");

                //Setup
                var tempGamesList = new List<Game>();
                XmlDocument xdoc = new XmlDocument();

                if (!File.Exists(dbPath)) File.Create(dbPath);

                //Load the xml
                xdoc.Load(dbPath);

                string name = string.Empty, image = string.Empty, desc = string.Empty, cloneof = string.Empty, crc = string.Empty,
                    manu = string.Empty, genre = string.Empty, rating = string.Empty;
                int enabled = 0;
                int year = 0;
                string index = string.Empty;
                var i = 0;
                var lastRom = string.Empty;

                try
                {
                    //Build the games list
                    foreach (XmlNode node in xdoc.SelectNodes("menu/game"))
                    {
                        name = node.SelectSingleNode("@name").InnerText;

                        char s = name[0];
                        char t;

                        if (lastRom != string.Empty)
                        {
                            t = lastRom[0];
                            if (char.ToLower(s) == char.ToLower(t))
                            {
                                index = string.Empty;
                                image = string.Empty;
                            }
                            else
                            {
                                index = "true";
                                image = char.ToLower(s).ToString();
                            }
                        }

                        if (node.SelectSingleNode("@enabled") != null)
                        {
                            if (node.SelectSingleNode("@enabled").InnerText != null)
                            {
                                enabled = Convert.ToInt32(node.SelectSingleNode("@enabled").InnerText);
                            }
                        }
                        else
                            enabled = 1;

                        if (!_systemName.Contains("Main Menu"))
                        {
                            desc = node.SelectSingleNode("description").InnerText;

                            if (node.SelectSingleNode("cloneof") != null)
                                cloneof = node.SelectSingleNode("cloneof").InnerText;
                            if (node.SelectSingleNode("crc") != null)
                                crc = node.SelectSingleNode("crc").InnerText;
                            if (node.SelectSingleNode("manufacturer") != null)
                                manu = node.SelectSingleNode("manufacturer").InnerText;
                            if (node.SelectSingleNode("year") != null)
                                if (!string.IsNullOrEmpty(node.SelectSingleNode("year").InnerText))
                                    Int32.TryParse(node.SelectSingleNode("year").InnerText, out year);

                            if (node.SelectSingleNode("genre") != null)
                            {
                                var genreName = node.SelectSingleNode("genre").InnerText;

                                genreName = genreName.Replace(@"\", "-");
                                genreName = genreName.Replace(@"/", "-");

                                genre = genreName;
                            }
                            if (node.SelectSingleNode("rating") != null)
                                rating = node.SelectSingleNode("rating").InnerText;
                        }

                        tempGamesList.Add(new Game(name, index, image, desc, cloneof, crc, manu, year, genre, rating, enabled, _systemName));

                        lastRom = name;
                        i++;
                    }

                    //Only sort by romname if it isn't main menu
                    if (!_systemName.Contains("Main Menu"))
                        tempGamesList.Sort((x, y) => x.RomName.CompareTo(y.RomName));

                    return tempGamesList;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        /// <summary>
        /// Deserializes a hyperspin favorites text file to favorites
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Favorite>> DeserializeFavoritesAsync()
        {
            return await Task.Run<IEnumerable<Favorite>>(() =>
           {
               var favoritesList = new List<Favorite>();

               try
               {
                   var favoriteTextFile = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName, "favorites.txt");

                   if (!File.Exists(favoriteTextFile)) return null;

                   using (StreamReader reader = new StreamReader(favoriteTextFile))
                   {
                       var line = string.Empty;
                       while ((line = reader.ReadLine()) != null)
                       {
                           favoritesList.Add(new Favorite { RomName = line });
                       }

                       return favoritesList;
                   }
               }
               catch (Exception) { throw; }
           });
        }

        /// <summary>
        /// Deserializes the genres asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<Genre>> DeserializeGenresAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes hyperspin systems from the main menu.xml
        /// <para/> Throws exception if process fails
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<MainMenu>> DeserializeMenusAsync()
        {
            return Task.Run<IEnumerable<MainMenu>>(() =>
            {
                var mainMenuXmlPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, "Main Menu", _databaseName + ".xml");

                if (!File.Exists(mainMenuXmlPath))
                    throw new FileNotFoundException($"Main menu xml doesn't exist: {mainMenuXmlPath}");

                var _systems = new List<MainMenu>();

                string[,] systems = GetSystems(mainMenuXmlPath);

                try
                {
                    systems[0, 0] = "Main Menu";
                    systems[0, 1] = "0";
                    _systems.Add(new MainMenu(systems[0, 0], Convert.ToInt32(systems[0, 1])));

                    for (int i = 1; i < systems.GetLength(0); i++)
                    {
                        try
                        {
                            //Add the enabled flag to the xml if null
                            if (systems[i, 1] == null) { systems[i, 1] = "1"; }

                            _systems.Add(new MainMenu(systems[i, 0], Convert.ToInt32(systems[i, 1])));

                        }
                        catch (Exception) { throw; }
                    }

                    return _systems;
                }
                catch (Exception) { throw; }
            });

        }

        /// <summary>
        /// Get genres from Hyperspins genre.xml
        /// </summary>
        /// <param name="genreXmlPath"></param>
        public async Task<IEnumerable<Genre>> GetGenresAsync(string genreXmlPath)
        {
            return await Task.Run(() =>
            {
                var genreList = new List<Genre>();

                using (var reader = new XmlTextReader(genreXmlPath))
                {
                    while (reader.Read())
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "game"))
                        {
                            if (reader.HasAttributes)
                            {
                                var genreName = reader.GetAttribute("name");

                                genreList.Add(new Genre { GenreName = genreName });
                            }
                        }
                    }

                    return genreList.AsEnumerable();
                }
            });
        }

        /// <summary>
        /// Serializes hyperspin games to xml asynchronously
        /// </summary>
        /// <param name="objectsToSerialize">The objects to serialize.</param>
        /// <returns></returns>
        public async Task<bool> SerializeAsync(IEnumerable<Game> objectsToSerialize, bool isMultiSystem = false)
        {
            if (!Directory.Exists(_frontEndPath))
                throw new DirectoryNotFoundException("Frontend path doesn't exist");

            var databasePath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName);

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            return await Task.Run(() =>
            {
                if (_databaseName != "Favorites")
                {
                    if (string.IsNullOrEmpty(_databaseName))
                    {
                        _databaseName = _systemName;
                    }
                }

                var dbXmlFileName = _databaseName + ".xml";
                string finalPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName, dbXmlFileName);

                //Create a folder if its a multisystem
                if (isMultiSystem)
                {
                    File.Create(databasePath + "\\_multisystem");
                }

                //Create the database file if doesnt exist
                //if (!File.Exists(finalPath))
                //{
                //    var file = File.Create(finalPath);
                //    file.Close();
                //}

                //Sort the incoming games
                var games = objectsToSerialize.ToList();
                games.Sort();

                //Set up serializer
                XmlSerializer serializer;
                var xmlNameSpace = new XmlSerializerNamespaces();
                xmlNameSpace.Add("", "");
                var xmlRootAttr = new XmlRootAttribute("menu");

                ///Serialize the games. Deal with Main Menus and normal game entries
                using (var txtWriter = new StreamWriter(finalPath))
                {
                    if (!_systemName.Contains("Main Menu"))
                    {
                        try
                        {
                            serializer = new XmlSerializer(typeof(List<Game>), xmlRootAttr);
                            serializer.Serialize(txtWriter, games, xmlNameSpace);
                        }
                        catch (Exception) { txtWriter.Close(); throw; }
                    }
                    else
                    {
                        var menuItems = new List<MainMenu>();

                        foreach (var game in games)
                        {
                            menuItems.Add(new MainMenu(game.RomName, game.GameEnabled));
                        }

                        serializer = new XmlSerializer(typeof(List<MainMenu>), xmlRootAttr);
                        serializer.Serialize(txtWriter, menuItems, xmlNameSpace);
                    }
                }

                return true;
            });
        }

        /// <summary>
        /// Serializes hypespin main menu items to xml asynchronously
        /// </summary>
        /// <param name="objectsToSerialize">The objects to serialize.</param>
        /// <returns></returns>
        public Task<bool> SerializeAsync(IEnumerable<MainMenu> objectsToSerialize, bool isMultiSystem = false)
        {
            return Task.Run(() =>
            {
                var finalPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, "Main Menu", _databaseName + ".xml");
                var databasePath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, "Main Menu");

                //objectsToSerialize.Remove(new MainMenu("Main Menu"));

                if (!Directory.Exists(databasePath))
                    Directory.CreateDirectory(databasePath);

                if (File.Exists(finalPath))
                {
                    if (File.Exists(finalPath + "BACKUP"))
                        File.Delete(finalPath + "BACKUP");

                    File.Copy(finalPath, finalPath + "BACKUP");
                }

                XmlSerializer serializer;
                using (var textWriter = new StreamWriter(finalPath))
                {
                    var xmlNameSpace = new XmlSerializerNamespaces();
                    var xmlRootAttr = new XmlRootAttribute("menu");
                    xmlNameSpace.Add("", "");

                    var menuItems = new List<MainMenu>();
                    foreach (var item in objectsToSerialize)
                    {
                        if (!item.Name.Contains("Main Menu"))
                            menuItems.Add(new MainMenu(item.Name, item.Enabled));
                    }

                    try
                    {
                        serializer = new XmlSerializer(typeof(List<MainMenu>), xmlRootAttr);
                        serializer.Serialize(textWriter, menuItems, xmlNameSpace);
                    }
                    catch (Exception) { throw; }

                    textWriter.Close();

                    return true;
                }
            });
        }

        /// <summary>
        /// Serializes the favorites from a gameslist to hyperspin text asynchronously
        /// </summary>
        /// <param name="games">The games to get favorites from</param>
        /// <returns></returns>
        public Task<bool> SerializeFavoritesAsync(IEnumerable<Game> games)
        {
            return Task.Run(() =>
            {
                if (games.Any(x => x.IsFavorite))
                {
                    var favoritesTxtPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName);

                    if (!Directory.Exists(favoritesTxtPath)) Directory.CreateDirectory(favoritesTxtPath);

                    //Delete the text file before stream
                    var faveTextFile = Path.Combine(favoritesTxtPath, "favorites.txt");
                    if (File.Exists(faveTextFile))
                        File.Delete(faveTextFile);

                    //Create the favorites text file
                    using (var sWriter = new StreamWriter(faveTextFile))
                    {
                        foreach (var favorite in games.Where(x => x.IsFavorite))
                        {
                            sWriter.WriteLine(favorite.RomName);
                        }

                        return true;
                    }
                }

                return false;
            });
        }

        /// <summary>
        /// Serializes the genres from a gamelist to seperate xmls in the database directory asynchronously.
        /// </summary>
        /// <param name="games">The games.</param>
        /// <returns></returns>
        public Task<bool> SerializeGenresAsync(IEnumerable<Game> games)
        {
            return Task.Run(async () =>
            {
                var genres = GetGenreNames(games);

                //Genre menu file
                if (!SerializeGenreMainMenu(genres))
                    throw new Exception($"Failed to serialze genre menu for {_systemName}");

                //Every genre gets its own xml
                foreach (var dbName in genres)
                {
                    if (dbName.GenreName != "")
                    {
                        var genreGames = games.Where(x => x.Genre == dbName.GenreName);

                        _databaseName = dbName.GenreName;

                        bool serializedGames = await SerializeAsync(genreGames);

                        if (!serializedGames)
                            throw new Exception($"Couldn't save genre games for {_databaseName}");
                    }
                }

                return true;
            });
        }

        #endregion

        #region Support Methods
        private string[,] GetSystems(string mainMenuXmlDatabase)
        {
            if (!File.Exists(mainMenuXmlDatabase))
                return new string[0, 0];

            string[,] systemsArray;

            using (XmlTextReader reader = new XmlTextReader(mainMenuXmlDatabase))
            {
                var menuName = Path.GetFileNameWithoutExtension(mainMenuXmlDatabase);
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(mainMenuXmlDatabase);
                int sysCount = xdoc.SelectNodes("menu/game").Count + 1;
                systemsArray = new string[sysCount, 2];
                int i = 0;
                systemsArray[i, 0] = menuName;
                systemsArray[i, 1] = "0";

                i++;
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "game"))
                        if (reader.HasAttributes)
                        {
                            systemsArray[i, 0] = reader.GetAttribute("name");
                            systemsArray[i, 1] = reader.GetAttribute("enabled");

                            i++;
                        }
                }
            }

            return systemsArray;
        }

        /// <summary>
        /// Pull all genre names from the incoming gamesList
        /// </summary>
        /// <param name="gamesList"></param>
        /// <returns></returns>
        private IEnumerable<Genre> GetGenreNames(IEnumerable<Game> gamesList)
        {
            var genres = gamesList
                .GroupBy(x => x.Genre)
                .Select(x => new Genre { GenreName = x.Key })
                .OrderBy(x => x.GenreName)
                .Distinct();

            var count = genres.Count();

            foreach (var genre in genres)
            {
                genre.GenreName = genre.GenreName.Replace("/", " ").Replace(@"\", " ");
            }

            return genres;
        }

        private bool SerializeGenreMainMenu(IEnumerable<Genre> genres)
        {
            var genreXml = "genre.xml";
            var finalPath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName, genreXml);
            var databasePath = Path.Combine(_frontEndPath, HyperspinRootPaths.Databases, _systemName);
            TextWriter textWriter = new StreamWriter(finalPath);

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            //Setup Xml
            XmlSerializer serializer;
            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add("", "");
            var xmlRootAttr = new XmlRootAttribute("menu");

            var menuItems = new List<MainMenu>();
            foreach (var item in genres)
            {
                if (item.GenreName != "")
                    menuItems.Add(new MainMenu(item.GenreName, 1));
            }

            try
            {
                serializer = new XmlSerializer(typeof(List<MainMenu>), xmlRootAttr);
                serializer.Serialize(textWriter, menuItems, xmlNameSpace);
            }
            catch { return false; }

            textWriter.Close();

            return true;
        }

        private async Task<IEnumerable<Game>> ScanFavorites(string frontEndPath, IEnumerable<MainMenu> systems)
        {
            var _hyperspinSerializer = new HyperspinSerializer(frontEndPath, "", "");
            var search = new XmlSearch<Game>();
            List<Game> games = new List<Game>();

            foreach (MainMenu system in systems)
            {
                var sysPath = Path.Combine(frontEndPath, HyperspinRootPaths.Databases, system.Name);
                var isMultiSystem = File.Exists(sysPath + "\\MultiSystem");

                // Dont want to be scanning the favorites of a multisystem
                if (system.Name != "Main Menu" && !isMultiSystem)
                {
                    //Change name and get favorites from text file async
                    _hyperspinSerializer.ChangeSystemAndDatabase(system.Name);
                    var favorites = await _hyperspinSerializer.DeserializeFavoritesAsync();

                    if(favorites != null)
                    {
                        var strfavorites = favorites.Select(x => x.RomName);

                        if (favorites.Count() > 0)
                        {
                            try
                            {
                                var faveGames = await search.Search(system.Name, Path.Combine(sysPath, $"{system.Name}.xml"), strfavorites, true);
                                games.AddRange(faveGames);
                            }
                            catch { }
                        }
                    }                    
                }
            }

            return games;
        }

        #endregion

    }
}
