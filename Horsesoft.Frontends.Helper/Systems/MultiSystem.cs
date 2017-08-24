using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
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
        private IRomMapperRl _romMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSystem"/> class and the games list
        /// </summary>
        public MultiSystem(IHyperspinSerializer hyperspinSerializer,
            ISystemCreator systemsCreator, IMediaCopier mediaCopier, MultiSystemOptions options)
        {
            Options = options;
            _hyperspinSerializer = hyperspinSerializer;
            _systemsCreator = systemsCreator;
            _mediaCopier = mediaCopier;
            _romMapper = new RomMapperRl();
        }

        #region Properties

        public MultiSystemOptions Options { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the multi system.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">Hyperspin serilaizer cannot be null</exception>
        /// <exception cref="Exception">Games has to be greater than 0</exception>
        public async Task<bool> CreateMultiSystem(IEnumerable<Game> games, string frontEndPath, string rlPath)
        {
            if (_hyperspinSerializer == null)
                throw new NullReferenceException("Hyperspin serilaizer cannot be null");

            if (games.Count() <= 0)
                throw new Exception("Games has to be greater than 0");

            if (string.IsNullOrWhiteSpace(Options.MultiSystemName)) throw new NullReferenceException("Multi system name cannot be empty");

            return await Task.Run(async () =>
            {
                //Get the systems used in this multisystem
                var systems = games.GroupBy(x => x.System).Distinct();

                //Create dirs and settings for hyperspin
                var result = await _systemsCreator.CreateSystem(Options.MultiSystemName);
                if (!result)
                {
                    throw new Exception($"Failed to create hyperspin defaults for {Options.MultiSystemName}");
                }

                //Copy settings if user wants to use their own
                UseTemplatedSettings(Options.MultiSystemName, frontEndPath);

                //Serialize the Multi system to xml
                if (!await _hyperspinSerializer.SerializeAsync(games))
                {
                    throw new Exception("Failed to serialize multi system games");
                }

                //Create multisystem folder and add a games.ini
                var dbPath = PathHelper.GetSystemDatabasePath(frontEndPath, Options.MultiSystemName) + "\\MultiSystem";
                Directory.CreateDirectory(dbPath);
                await _romMapper.CreateGamesIniAsync(games, dbPath);

                //Serialze favorites               
                if (!await _hyperspinSerializer.SerializeFavoritesAsync(games))
                {
                }

                //Create genres for the system
                if (Options.CreateGenres)
                {
                    if (!await _hyperspinSerializer.SerializeGenresAsync(games))
                    {
                        throw new Exception("Failed to serialize genres for multi system games");
                    }
                }

                //Copy the media or create symbolic links.
                if (Options.CopyMedia)
                {
                    await _mediaCopier.CopyAllMediaAsync(games, Options.MultiSystemName, Options.CreateSymbolicLinks);
                }

                //Create Rl rom mapping
                if (Options.CreateRomMap)
                {
                    var gamesIniPath = Path.Combine(rlPath, Paths.RocketLauncherPaths.Settings, Options.MultiSystemName);

                    await _romMapper.CreateGamesIniAsync(games, gamesIniPath);
                }

                return true;
            });
        }

        #endregion

        #region Support Methods

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
