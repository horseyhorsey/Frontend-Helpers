using Frontends.Models.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Serialization;
using Horsesoft.Frontends.Helper.Settings;
using Horsesoft.Frontends.Helper.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Systems
{
    public class MultiSystem : IMultiSystem
    {        
        private IHyperspinSerializer _hyperspinSerializer;
        private ISystemCreator _systemsCreator;
        private IMediaCopier _mediaCopier;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSystem"/> class and the games list
        /// </summary>
        public MultiSystem(IHyperspinSerializer hyperspinSerializer, 
            ISystemCreator systemsCreator, IMediaCopier mediaCopier, MultiSystemOptions options)
        {
            Games = new List<Game>();

            Options = options;

            _hyperspinSerializer = hyperspinSerializer;
            _systemsCreator = systemsCreator;
            _mediaCopier = mediaCopier;
        }

        /// <summary>
        /// Gets the games.
        /// </summary>
        public List<Game> Games { get; }

        public MultiSystemOptions Options { get; set; }

        #region Public Methods

        /// <summary>
        /// Adds the specified game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public bool Add(Game game)
        {
            bool result = false;
            if (!GameExists(game))
            {
                Games.Add(game);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Creates the multi system.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Hyperspin serilaizer cannot be null</exception>
        /// <exception cref="Exception">Games has to be greater than 0</exception>
        public async Task<bool> CreateMultiSystem(string frontEndPath)
        {
            if (_hyperspinSerializer == null)
                throw new NullReferenceException("Hyperspin serilaizer cannot be null");

            if (Games.Count <= 0)
                throw new Exception("Games has to be greater than 0");

            if (string.IsNullOrWhiteSpace(Options.MultiSystemName)) throw new NullReferenceException("Multi system name cannot be empty");

            return await Task.Run(async () =>
            {
                //Create dirs and settings for hyperspin
                var result =  await _systemsCreator.CreateSystem(Options.MultiSystemName);                
                if (!result)
                {
                    throw new Exception($"Failed to create hyperspin defaults for {Options.MultiSystemName}");
                }

                //Copy settings if user wants to use their own
                UseTemplatedSettings(Options.MultiSystemName, frontEndPath);

                //Serialize the Multi system to xml
                if (!await _hyperspinSerializer.SerializeAsync(Games))
                {
                    throw new Exception("Failed to serialize multi system games");
                }

                //Create genres for the system
                if (Options.CreateGenres)
                {
                    if (!await _hyperspinSerializer.SerializeGenresAsync(Games))
                    {
                        throw new Exception("Failed to serialize genres for multi system games");
                    }
                }

                if (Options.CopyMedia)
                {
                    await _mediaCopier.CopyAllMediaAsync(Games, Options.MultiSystemName, Options.CreateSymbolicLinks);
                }

                return true;
            });
        }

        /// <summary>
        /// Removes the specified game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public bool Remove(Game game)
        {
            bool result = false;

            if (GameExists(game))
            {
                Games.Remove(game);
                result = true;
            }

            return result;
        }

        public async Task<bool> ScanFavorites(string frontEndPath, IEnumerable<MainMenu> systems)
        {
            var hsPath = frontEndPath;

            foreach (MainMenu system in systems)
            {
                var isMultiSystem = File.Exists(Path.Combine(
                    hsPath, Root.Databases, system.Name, "_multisystem"));

                // Dont want to be scanning the favorites of a multisystem
                if (system.Name != "Main Menu" && !isMultiSystem)
                {
                    //Change name and get favorites
                    _hyperspinSerializer.ChangeSystemAndDatabase(system.Name);
                    var favorites = await _hyperspinSerializer.DeserializeFavoritesAsync();

                    if (favorites.Count() > 0)
                    {
                        //var games = _xmlService.SearchRomStringsListFromXml(favoritesList, system.Name, hsPath);

                        //foreach (Game game in games)
                        //{
                        //    await ScanGames(game, tempGames);
                        //}                        
                    }
                }
            }

            return true;
        }

        private Task ScanGames(Game game, List<Game> tempGames)
        {
            return Task.Run(() =>
            {
                if (!tempGames.Exists(x => x.RomName == game.RomName))
                {
                    tempGames.Add(game);
                }

                tempGames.Sort();

                //_multiSystemRepo.MultiSystemList.Clear();

                //foreach (Game sortedGame in tempGames)
                //{
                //    _multiSystemRepo.MultiSystemList.Add(sortedGame);
                //}
            });
        }

        #endregion

        #region Support Methods
        private bool GameExists(Game game)
        {
            return Games.Any(x => x.RomName == game.RomName) ? true : false;
        }

        /// <summary>
        /// Copies the template file if exists or uses the default settings.
        /// </summary>
        /// <param name="multiSystemName">Name of the multi system.</param>
        /// <param name="frontEndPath">The front end path.</param>
        private void UseTemplatedSettings(string multiSystemName, string frontEndPath)
        {
            if (File.Exists(Options.SettingsTemplateFile))
            {
                var settingsIniPath = Path.Combine(frontEndPath, Root.Settings, multiSystemName + ".ini");

                if (File.Exists(settingsIniPath))
                    File.Delete(settingsIniPath);

                File.Copy(Options.SettingsTemplateFile, settingsIniPath);
            }
        }

        #endregion
    }
}
