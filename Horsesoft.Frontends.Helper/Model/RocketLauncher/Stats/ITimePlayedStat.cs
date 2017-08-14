using System;

namespace Horsesoft.Frontends.Helper.Model.RocketLauncher.Stats
{
    internal interface ITimePlayedStat
    {
        TimeSpan? TimePlayed { get; }
    }
}
