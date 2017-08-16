using System;

namespace Horsesoft.Frontends.Models.RocketLauncher.Stats
{
    public class TopTenTimesPlayedStat : GlobalStatBase, ITimesPlayedStat
    {
        public TopTenTimesPlayedStat(string system, string name, string desc, string timesPlayed) : base(system, name, desc)
        {
            if (!string.IsNullOrWhiteSpace(timesPlayed))
                TimesPlayed = Convert.ToInt32(timesPlayed);
        }

        public int TimesPlayed { get; }
    }
}
