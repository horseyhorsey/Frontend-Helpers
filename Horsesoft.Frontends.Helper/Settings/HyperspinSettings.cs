using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Hs.Hypermint.Services.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Frontends.Models.Hyperspin.Settings;
using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;

namespace Horsesoft.Frontends.Helper.Settings
{
    public class HyperspinSettings : ISettings
    {
        private IFrontend _frontend;
        private string _systemName;

        public SystemSetting Settings { get; set; }

        #region Support Methods
        public HyperspinSettings(IFrontend frontend, string systemName)
        {
            _frontend = frontend;
            _systemName = systemName;
            Settings = new SystemSetting();
        }

        /// <summary>
        /// Deserializes a settings ini asynchronous.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        public Task<bool> DeserializeSettingsAsync(string systemName = null)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(systemName))
                {
                    if (string.IsNullOrWhiteSpace(_systemName))
                        return false;
                    else
                        systemName = _systemName;
                }

                var iniPath = Path.Combine(_frontend.Path, Root.Settings, $"{systemName}.ini");

                if (!File.Exists(iniPath))
                    throw new FileNotFoundException($"Ini not found: {iniPath}");

                try
                {
                    var ini = new IniFile();
                    ini.Load(iniPath);

                    Settings.ExeInfo = GetExeInfo(ini);

                    return true;
                }
                catch
                {
                    return false;
                }
            });

        } 
        #endregion

        /// <summary>
        /// Gets the executable information.
        /// </summary>
        /// <param name="ini">The ini.</param>
        /// <returns></returns>
        private ExeInfo GetExeInfo(IniFile ini)
        {
            var exeInfo = new ExeInfo();
            var sectionName = "exe info";

            exeInfo.Path = ini.GetKeyValue(sectionName, "path");
            exeInfo.Exe = ini.GetKeyValue(sectionName, "exe");
            exeInfo.UseRomPath = Convert.ToBoolean(ini.GetKeyValue(sectionName, "userompath"));
            exeInfo.RomPath = ini.GetKeyValue(sectionName, "rompath");
            exeInfo.RomExtension = ini.GetKeyValue(sectionName, "romextension");
            exeInfo.Parameters = ini.GetKeyValue(sectionName, "parameters");
            exeInfo.SearchSubFolders = Convert.ToBoolean(ini.GetKeyValue(sectionName, "searchsubfolders"));
            exeInfo.Per_Game_Modules = Convert.ToBoolean(ini.GetKeyValue(sectionName, "Per_Game_Modules"));
            exeInfo.PcGame = Convert.ToBoolean(ini.GetKeyValue(sectionName, "pcgame"));
            exeInfo.WinState = ini.GetKeyValue(sectionName, "pcgame");
            exeInfo.HyperLaunch = Convert.ToBoolean(ini.GetKeyValue(sectionName, "hyperlaunch"));
            exeInfo.MultiGame_Enabled = Convert.ToBoolean(ini.GetKeyValue(sectionName, "MultiGame_Enabled"));

            return exeInfo;
        }

        /// <summary>
        /// Serializes the settings.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SerializeSettings()
        {
            throw new NotImplementedException();
        }
    }
}
