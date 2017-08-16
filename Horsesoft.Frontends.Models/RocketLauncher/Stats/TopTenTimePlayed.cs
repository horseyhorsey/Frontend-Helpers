using System;

namespace Frontends.Models.RocketLauncher.Stats
{
    public class TopTenTimePlayedStat : GlobalStatBase, ITimePlayedStat
    {
        public TopTenTimePlayedStat(string system, string name, string desc, string timePlayed) : base( system, name, desc)
        {
            if (!string.IsNullOrWhiteSpace(timePlayed))
                TimePlayed = TimeSpan.FromSeconds(double.Parse(timePlayed));
        }

        public TimeSpan? TimePlayed { get; }
    }
}
