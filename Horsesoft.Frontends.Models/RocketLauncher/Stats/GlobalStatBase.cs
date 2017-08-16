namespace Horsesoft.Frontends.Models.RocketLauncher.Stats
{
    public abstract class GlobalStatBase : IGlobalStat
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalStatBase"/> class.
        /// </summary>
        /// <param name="system">The system.</param>
        /// <param name="name">The name.</param>
        /// <param name="desc">The desc.</param>
        public GlobalStatBase(string system, string name, string desc)
        {
            System = system;
            Name = name;
            Description = desc;
        }

        /// <summary>
        /// Gets or sets the SystemName.
        /// </summary>
        public string System { get; }

        /// <summary>
        /// Gets or sets the RomName.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; }

    }
}
