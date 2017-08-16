using Horsesoft.Frontends.Helper.Common;
using Horsesoft.Frontends.Helper.Common.Attributes;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using Horsesoft.Frontends.Helper.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Horsesoft.Frontends.Helper.Hyperspin
{
    [Frontend(Model.FrontendType.Hyperspin)]
    public class HyperspinFrontend : FrontendBase, IHyperspinFrontend
    {
        public HyperspinFrontend()
        {
        }

        #region Public Methods
        public override bool Launch()
        {
            Console.WriteLine($"Launching {nameof(HyperspinFrontend)}...");

            return true;
        }

        public override Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName)
        {
            return Task.Run(() =>
            {
                if (systemName.Contains("Main Menu"))
                    throw new Exception("Can't get databases for a Main Menu");

                return GetDatabaseFilesForSystem(systemName, "*.xml");
            });
        }

        #endregion
    }
}
