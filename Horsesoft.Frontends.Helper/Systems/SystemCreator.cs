﻿using System.IO;
using System.Reflection;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System.Threading.Tasks;
using Frontends.Models;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Systems
{
    public class SystemCreator : ISystemCreator
    {
        IFrontend _frontend;

        public SystemCreator(IFrontend frontend)
        {
            _frontend = frontend;
        }

        #region Public Methods
        public async Task<bool> CreateSystem(string systemName, string existingDb = "")
        {
            return await Task.Run(() =>
            {
                try
                {
                    var frontendType = _frontend.GetFrontendType();

                    switch (frontendType)
                    {
                        case FrontendType.Hyperspin:
                            CreateSystemForHyperspin(systemName, existingDb);
                            break;
                        case FrontendType.Hyperpin:
                            break;
                        case FrontendType.RocketLauncher:
                            break;
                        case FrontendType.PinballX:
                            break;
                        default:
                            break;
                    }

                    return true;
                }
                catch { return false; }
            });
        }
        #endregion

        #region Support Methods

        /// <summary>
        /// Creates a new system for hyperspin.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="existingDb">if set to <c>true</c> [existing database].</param>
        private void CreateSystemForHyperspin(string systemName, string existingDb = "")
        {
            var systemSettingsPath = Path.Combine(_frontend.Path, Paths.Hyperspin.HyperspinRootPaths.Settings);
            var systemSettingsIniPath = Path.Combine(systemSettingsPath, $"{systemName}.ini");

            //Generate settings from the embedded resource if not available
            if (!File.Exists(systemSettingsIniPath))
            {
                //Create settings path if not found
                if (!Directory.Exists(systemSettingsPath)) Directory.CreateDirectory(systemSettingsPath);

                GenerateDefaultHyperspinSettings(systemSettingsIniPath);
            }

            CreateDirectoriesForHyperspin(systemName);

            CreateEmptyHyperspinDb(systemName, existingDb);
        }

        /// <summary>
        /// Creates an empty system database xml for hyperspin.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="existingDb">if set to <c>true</c> [existing database]. Leave empty string</param>
        private void CreateEmptyHyperspinDb(string systemName, string existingDb = "")
        {
            var newSystemDbPath = Path.Combine(_frontend.Path, HyperspinRootPaths.Databases, systemName);
            var dbFile = Path.Combine(newSystemDbPath, systemName + ".xml");

            if (string.IsNullOrWhiteSpace(existingDb)) //Create a blank database with new system name.
            {                
                File.Create(dbFile).Close();
            }
            else // Create a new database from an existing hyperspin xml
            {
                try
                {
                    if (!File.Exists(existingDb))
                    {
                        throw new FileNotFoundException("Existing hyperspin database not found");
                    }                    
                }
                catch
                {
                    File.Create(dbFile).Close();
                }

                try
                {
                    File.Delete(dbFile);
                    File.Copy(existingDb, dbFile);
                }
                catch (System.Exception ex)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Generates a default hyperspin settings at the given path
        /// </summary>
        /// <param name="systemSettingsIniPath">The system settings ini path.</param>
        private void GenerateDefaultHyperspinSettings(string systemSettingsIniPath)
        {            
            //Load templated hyperspin settings ini from embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Horsesoft.Frontends.Helper.Resources.systemSettings.ini";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            using (var textFile = File.CreateText(systemSettingsIniPath))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    textFile.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Generate directorys for a hyperspin system
        /// </summary>
        /// <param name="hsPath">Path to hyperspin</param>
        /// <param name="systemName">The system name</param>
        private void CreateDirectoriesForHyperspin(string systemName)
        {
            var newSystemMediaPath = Path.Combine(_frontend.Path, HyperspinRootPaths.Media, systemName);
            var newSystemDbPath = Path.Combine(_frontend.Path, HyperspinRootPaths.Databases, systemName);

            //Media Directories
            for (int i = 1; i < 5; i++)
            {
                Directory.CreateDirectory(newSystemMediaPath + "\\Images\\Artwork" + i);
            }
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.Backgrounds);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.GenreBackgrounds);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.GenreWheel);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.Letters);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.Pointer);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.Special);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Images.Wheels);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + HyperspinRootPaths.Themes);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Sound.BackgroundMusic);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Sound.SystemExit);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Sound.SystemStart);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + Sound.WheelSounds);
            Directory.CreateDirectory(newSystemMediaPath + "\\" + HyperspinRootPaths.Video);

            //Create the database directory
            if (!Directory.Exists(newSystemDbPath))
                Directory.CreateDirectory(newSystemDbPath);
        } 

        #endregion
    }
}
