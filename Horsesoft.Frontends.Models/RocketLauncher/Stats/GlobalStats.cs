using System.Collections.Generic;

namespace Frontends.Models.RocketLauncher.Stats
{
    public class GlobalStats
    {
        public GlobalStats()
        {
            LastPlayedGames = new List<LastPlayedGameStat>(10);
            TopTenSystemMostPlayed = new List<SystemTopTenStat>(10);
            TopTenTimePlayed = new List<TopTenTimePlayedStat>(10);
            TopTenTimesPlayed = new List<TopTenTimesPlayedStat>(10);
            TopTenAverageTimePlayed = new List<TopTenAverageTimePlayedStat>(10);
        }

        public List<LastPlayedGameStat> LastPlayedGames { get; set; }
        public List<SystemTopTenStat> TopTenSystemMostPlayed { get; set; }
        public List<TopTenTimePlayedStat> TopTenTimePlayed { get; set; }
        public List<TopTenTimesPlayedStat> TopTenTimesPlayed { get; set; }
        public List<TopTenAverageTimePlayedStat> TopTenAverageTimePlayed { get; set; }        
    }
}
