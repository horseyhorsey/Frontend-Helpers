using Horsesoft.Frontends.Helper.Model.Hyperspin.Settings;

namespace Horsesoft.Frontends.Helper.Model.Hyperspin
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
