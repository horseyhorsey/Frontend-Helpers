using System;

namespace Frontends.Models.RocketLauncher.Stats
{
    internal interface ITimePlayedStat
    {
        TimeSpan? TimePlayed { get; }
    }
}
