using Frontends.Models.Hyperspin;
using Frontends.Models.RocketLauncher.Stats;
using Frontends.Models.Stats;
using Hs.Hypermint.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Serialization
{
    internal class RocketStatsSerializer
    {
        #region Fields
        private IniFile _ini = new IniFile();

        string[] _genStats = { "General", "TopTen_Time_Played", "TopTen_Times_Played", "Top_Ten_Average_Time_Played" }; 
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the single game stats by RomName
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        internal GameStat GetSingleGameStats(string frontendPath, Game game)
        {
            var system = game.System;
            var romName = game.RomName;
            var iniPath = Path.Combine(frontendPath, Paths.RocketLauncherMediaPaths.Statistics, system + ".ini");

            if (!File.Exists(iniPath)) return null;

            _ini.Load(iniPath);

            var i = _ini.GetSection(romName);
            if (i == null)
                return new GameStat();

            var gameStat = new GameStat();
            gameStat.TimesPlayed = Convert.ToInt32(_ini.GetKeyValue(romName, "Number_of_Times_Played"));
            gameStat.LastTimePlayed = Convert.ToDateTime(_ini.GetKeyValue(romName, "Last_Time_Played"));

            var avgTime = TimeSpan.Parse(_ini.GetKeyValue(romName, "Average_Time_Played")).Days;
            gameStat.AvgTimePlayed = new TimeSpan(0, 0, avgTime);

            try
            {
                gameStat.TotalTimePlayed = TimeSpan.Parse(_ini.GetKeyValue(romName, "Total_Time_Played"));
            }
            catch (Exception) { }

            return gameStat;
        }

        /// <summary>
        /// Gets the global stats from the global stats ini.
        /// </summary>
        /// <param name="globalStatsIni">The global stats ini.</param>
        /// <returns></returns>
        internal async Task<GlobalStats> GetGlobalStatsAsync(string frontendPath)
        {
            var globalStatsIni = Path.Combine(frontendPath, Paths.RocketLauncherMediaPaths.Statistics, "Global Statistics.ini");

            if (!File.Exists(globalStatsIni)) throw new FileNotFoundException("Cannot find a Global Stats.ini");

            return await Task.Run(() =>
            {
                var globalStats = new GlobalStats();

                string sysStatIniName = Path.GetFileNameWithoutExtension(globalStatsIni);

                _ini.Load(globalStatsIni);
                int count = _ini.Sections.Count;

                foreach (IniFile.IniSection s in _ini.Sections)
                {
                    string sectionName = s.Name.ToString();

                    if (sectionName.ToLower() != "general")
                    {
                        switch (sectionName)
                        {
                            case "Last_Played_Games":
                                GetLastPlayedGameStats(sectionName, globalStats);
                                break;
                            case "TopTen_System_Most_Played":
                                GetTopTenSystemsMostPlayed(sectionName, globalStats);
                                break;
                            case "TopTen_Time_Played":
                                GetTopTenTimePlayed(sectionName, globalStats);
                                break;
                            case "TopTen_Times_Played":
                                GetTopTenTimesPlayed(sectionName, globalStats);
                                break;
                            case "Top_Ten_Average_Time_Played":
                                GetTopTenAveragePlayed(sectionName, globalStats);
                                break;
                            default:
                                break;
                        }
                    }
                }

                return globalStats;
            });
        }

        [Obsolete("Old methods")]
        internal Dictionary<string, Stats> GetAllGlobal(string globalStatsIni)
        {
            if (!File.Exists(globalStatsIni)) return null;

            string sysStatIniName = Path.GetFileNameWithoutExtension(globalStatsIni);

            var statList = new Stats();
            IniFile ini = new IniFile();
            ini.Load(globalStatsIni);

            var stats = new Dictionary<string, Stats>();

            foreach (IniFile.IniSection section in ini.Sections)
            {
                string sectionName = section.Name.ToString();

                stats.Add(sectionName, new Stats());

                switch (sectionName)
                {
                    case "Last_Played_Games":
                        string[] keys = new string[section.Keys.Count];

                        int i = 0;
                        foreach (IniFile.IniSection.IniKey item in section.Keys)
                        {
                            keys[i] = item.Name;

                            i++;
                        }
                        Array.Sort(keys);

                        foreach (var key in keys)
                        {
                            stats[sectionName].Add(new GameStat() { GlobalStatKey = key });
                        }
                        break;
                    default:
                        break;
                }

            }

            return stats;

        }

        [Obsolete]
        /// <summary>
        /// Gets the stats for system asynchronous.
        /// </summary>
        /// <param name="frontendPath">The frontend path.</param>
        /// <param name="menuSystem">The menu system.</param>
        /// <returns></returns>
        internal async Task<IEnumerable<GameStat>> GetStatsForSystemAsync(string frontendPath, MainMenu menuSystem)
        {
            return await Task.Run(()  =>
            {
                return GetGameStats(frontendPath, menuSystem);
            });
        }        

        #endregion

        #region Support Methods
        private void GetTopTenAveragePlayed(string sectionName, GlobalStats globalStats)
        {
            globalStats.TopTenAverageTimePlayed.Clear();

            for (int i = 1; i < 11; i++)
            {
                var name = GetName(i, sectionName);
                var sys = GetSystem(i, sectionName);
                var desc = GetDescription(i, sectionName);
                var timePlayed = GetTimePlayed(i, sectionName);

                var topTenTimesPlayed = new TopTenAverageTimePlayedStat(sys, name, desc, timePlayed);

                globalStats.TopTenAverageTimePlayed.Add(topTenTimesPlayed);

            }
        }

        private void GetTopTenTimesPlayed(string sectionName, GlobalStats globalStats)
        {
            globalStats.TopTenTimesPlayed.Clear();

            for (int i = 1; i < 11; i++)
            {
                var name = GetName(i, sectionName);
                var sys = GetSystem(i, sectionName);
                var desc = GetDescription(i, sectionName);
                var timesPlayed = _ini.GetKeyValue(sectionName, $"{i}_TimesPlayed");

                var topTenTimesPlayed = new TopTenTimesPlayedStat(sys, name, desc, timesPlayed);

                globalStats.TopTenTimesPlayed.Add(topTenTimesPlayed);

            }
        }

        private void GetTopTenTimePlayed(string sectionName, GlobalStats globalStats)
        {
            globalStats.TopTenTimePlayed.Clear();

            for (int i = 1; i < 11; i++)
            {
                var name = GetName(i, sectionName);
                var sys = GetSystem(i, sectionName);
                var desc = GetDescription(i, sectionName);
                var timePlayed = GetTimePlayed(i, sectionName);

                var topTenPlayed = new TopTenTimePlayedStat(sys, name, desc, timePlayed);

                globalStats.TopTenTimePlayed.Add(topTenPlayed);
            }
        }

        private void GetTopTenSystemsMostPlayed(string sectionName, GlobalStats globalStats)
        {
            globalStats.TopTenSystemMostPlayed.Clear();

            for (int i = 1; i < 11; i++)
            {
                var name = GetName(i, sectionName);
                var timePlayed = GetTimePlayed(i, sectionName);

                var systemTopTenStat = new SystemTopTenStat(name, timePlayed);

                globalStats.TopTenSystemMostPlayed.Add(systemTopTenStat);
            }
        }

        private void GetLastPlayedGameStats(string sectionName, GlobalStats globalStats)
        {
            globalStats.LastPlayedGames.Clear();

            for (int i = 1; i < 11; i++)
            {
                var name = GetName(i, sectionName);
                var sys = GetSystem(i, sectionName);
                var desc = GetDescription(i, sectionName);
                var date = _ini.GetKeyValue(sectionName, $"{i}_Date");

                var lastPlayedStat = new LastPlayedGameStat(sys, name, desc, date);

                globalStats.LastPlayedGames.Add(lastPlayedStat);
            }
        }

        private IEnumerable<GameStat> GetGameStats(string frontendPath, MainMenu menuSystem)
        {
            var iniPath = Path.Combine(frontendPath, Paths.RocketLauncherMediaPaths.Statistics, menuSystem.Name + ".ini");

            if (!File.Exists(iniPath)) throw new FileNotFoundException();

            var statList = new List<GameStat>();

            _ini.Load(iniPath);
            int count = _ini.Sections.Count;

            foreach (IniFile.IniSection s in _ini.Sections)
            {
                string section = s.Name.ToString();
                if (_genStats[0] != section)
                    if (_genStats[1] != section)
                        if (_genStats[2] != section)
                            if (_genStats[3] != section)
                            {
                                if (section == GeneralStats.General.ToString())
                                {
                                }
                                else
                                {
                                    var gameStat = new GameStat();
                                    gameStat.Rom = section;

                                    gameStat.SystemName = menuSystem.Name;
                                    try
                                    {
                                        gameStat.TimesPlayed = Convert.ToInt32(_ini.GetKeyValue(section, "Number_of_Times_Played"));

                                    }
                                    catch (Exception)
                                    {

                                    }

                                    try
                                    {
                                        gameStat.LastTimePlayed = Convert.ToDateTime(_ini.GetKeyValue(section, "Last_Time_Played"));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    try
                                    {

                                        //AvgTimePlayed = ;
                                        var avgTime = TimeSpan.Parse(_ini.GetKeyValue(section, "Average_Time_Played")).Days;
                                        gameStat.AvgTimePlayed = new TimeSpan(0, 0, avgTime);
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    try
                                    {
                                        var TotalTime = TimeSpan.Parse(_ini.GetKeyValue(section, "Total_Time_Played")).Days;

                                        gameStat.TotalTimePlayed = new TimeSpan(0, 0, TotalTime);
                                        gameStat.TotalOverallTime = gameStat.TotalOverallTime + gameStat.TotalTimePlayed;


                                    }
                                    catch (Exception)
                                    {


                                    }

                                    statList.Add(new GameStat
                                    {
                                        AvgTimePlayed = gameStat.AvgTimePlayed,
                                        LastTimePlayed = gameStat.LastTimePlayed,
                                        TimesPlayed = gameStat.TimesPlayed,
                                        Rom = gameStat.Rom,
                                        TotalTimePlayed = gameStat.TotalTimePlayed,
                                        TotalOverallTime = gameStat.TotalOverallTime,
                                        SystemName = gameStat.SystemName
                                    });
                                }
                            }
            }

            return statList.AsEnumerable();
        }

        #region Common Ini Methods
        /// <summary>
        /// Gets the description from ini section and id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private string GetDescription(int id, string sectionName) => _ini.GetKeyValue(sectionName, $"{id}_Description");

        /// <summary>
        /// Gets the system from ini section and id
        /// </summary>
        /// <param name="i">The identifier</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private string GetSystem(int id, string sectionName) => _ini.GetKeyValue(sectionName, $"{id}_System");

        /// <summary>
        /// Gets the top ten time played.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private string GetTimePlayed(int id, string sectionName) => _ini.GetKeyValue(sectionName, $"{id}_Time_Played");

        /// <summary>
        /// Gets the name from ini section and id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="section">The section.</param>
        private string GetName(int id, string section) => _ini.GetKeyValue(section, $"{id}_Name");
        #endregion 

        #endregion
    }
}
