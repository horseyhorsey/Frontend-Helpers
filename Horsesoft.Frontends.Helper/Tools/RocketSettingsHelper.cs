﻿using Hs.Hypermint.Services.Helpers;
using System;
using System.IO;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class RocketSettingsHelper
    {
        #region Public Methods
        public static string GetDefaultEmulator(string rlPath, string systemName)
        {
            string section = "ROMS";
            string key = "Default_Emulator";

            var iniFile = BuildEmuIniPath(rlPath, systemName);

            var defaultEmu = GetIniValue(iniFile, section, key);

            return defaultEmu ?? "";
        }

        public static string[] GetRomExtensions(string rlPath, string emuName)
        {
            string section = emuName;

            string key = "Rom_Extension";

            var iniFile = BuildGlobalEmuIniPath(rlPath);

            var extensions = GetIniValue(iniFile, section, key).Split('|');

            return extensions;
        }

        public static string[] GetRomPaths(string rlPath, string systemName)
        {
            string section = "ROMS";
            string key = "Rom_Path";

            var iniFile = BuildEmuIniPath(rlPath, systemName);

            var paths = GetIniValue(iniFile, section, key).Split('|');

            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].Contains(@"..\") || paths[i].Contains(@".\"))
                {
                    var romPath = new Uri(paths[i], UriKind.RelativeOrAbsolute);                    
                    var comb = Path.Combine(rlPath, romPath.OriginalString);
                    var combined = new Uri(comb, UriKind.RelativeOrAbsolute);                    
                    paths[i] = combined.LocalPath;                    
                }
            }

            return paths;
        } 
        #endregion

        #region Support Methods
        private static string GetIniValue(string iniFile, string section, string key)
        {
            if (!File.Exists(iniFile)) return null;

            IniFile ini = new IniFile();

            ini.Load(iniFile);

            return ini.GetKeyValue(section, key);
        }

        private static string BuildEmuIniPath(string rlPath, string systemName) =>
            rlPath + "\\Settings\\" + systemName + "\\Emulators.ini";

        private static string BuildGlobalEmuIniPath(string rlPath) =>
            rlPath + "\\Settings\\Global Emulators.ini"; 
        #endregion
    }
}
