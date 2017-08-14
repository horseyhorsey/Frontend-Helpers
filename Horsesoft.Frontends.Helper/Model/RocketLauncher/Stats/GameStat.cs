using System;

namespace Horsesoft.Frontends.Helper.Model.RocketLauncher.Stats
{
    public class GameStat
    {
        public string GlobalStatKey { get; set; }
        public static string StatsPath { get; set; }
        public int TimesPlayed { get; set; }
        public DateTime LastTimePlayed { get; set; }
        public TimeSpan AvgTimePlayed { get; set; }
        public TimeSpan TotalTimePlayed { get; set; }
        public string SystemName { get; set; }
        public string Rom { get; set; }
        public TimeSpan TotalOverallTime;

        public override string ToString()
        {
            return $"{SystemName} - {Rom} + {TimesPlayed}";
        }
    }
}
