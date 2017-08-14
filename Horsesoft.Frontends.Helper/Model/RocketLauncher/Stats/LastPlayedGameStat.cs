using System;

namespace Horsesoft.Frontends.Helper.Model.RocketLauncher.Stats
{
    public class LastPlayedGameStat : GlobalStatBase
    {
        public LastPlayedGameStat(string system, string name, string desc, string datePlayed) : 
            base(system, name, desc)
        {
            Date = DateTime.Parse(datePlayed);
        }

        public DateTime? Date { get; }
    }
}
