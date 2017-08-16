using Horsesoft.Frontends.Models.Hyperspin.Settings;

namespace Horsesoft.Frontends.Models.Hyperspin
{
    public class SystemSetting
    {
        public SystemSetting()
        {
            ExeInfo = new ExeInfo();
        }

        /// <summary>
        /// Gets or sets the ExeInfo. Used for launching variables.
        /// </summary>
        public ExeInfo ExeInfo { get; set; }
    }
}
