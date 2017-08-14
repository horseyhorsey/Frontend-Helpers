namespace Horsesoft.Frontends.Helper.Model.RocketLauncher.Stats
{
    public interface IGlobalStat : IStat
    {
        string System { get; }

        string Description { get; }
    }
}
