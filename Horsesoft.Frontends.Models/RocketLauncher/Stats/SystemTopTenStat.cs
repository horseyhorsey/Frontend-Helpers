using System;

namespace Horsesoft.Frontends.Models.RocketLauncher.Stats
{
    public class SystemTopTenStat : IStat, ITimePlayedStat
    {
        public string Name { get; }

        public TimeSpan? TimePlayed { get; }

        public SystemTopTenStat(string name, string timePlayed)
        {
            Name = name;

            if (!string.IsNullOrWhiteSpace(timePlayed))
                TimePlayed = TimeSpan.FromSeconds(double.Parse(timePlayed));
        }
    }
}
