namespace Horsesoft.Frontends.Models.RocketLauncher.Stats
{
    public interface IGlobalStat : IStat
    {
        string System { get; }

        string Description { get; }
    }
}
