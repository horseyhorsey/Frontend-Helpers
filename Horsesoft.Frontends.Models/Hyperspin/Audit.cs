namespace Frontends.Models.Hyperspin
{
    /// <summary>
    /// Global media audits
    /// </summary>
    public abstract class Audit
    {
        #region Properties

        private string romName;
        public string RomName
        {
            get { return romName; }
            set { romName = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool HaveWheel { get; set; }
        public bool HaveTheme { get; set; }
        public bool HaveVideo { get; set; }
        public bool HaveBGMusic { get; set; }
        #endregion
    }

}
