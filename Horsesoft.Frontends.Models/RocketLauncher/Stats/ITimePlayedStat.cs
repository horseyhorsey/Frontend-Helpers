using System;

namespace Horsesoft.Frontends.Models.RocketLauncher.Stats
{
    internal interface ITimePlayedStat
    {
        TimeSpan? TimePlayed { get; }
    }
}
