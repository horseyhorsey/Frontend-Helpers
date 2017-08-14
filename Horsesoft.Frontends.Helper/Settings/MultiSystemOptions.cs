namespace Horsesoft.Frontends.Helper.Settings
{
    /// <summary>
    /// Options for creating a Hyperspin multi system
    /// </summary>
    public class MultiSystemOptions
    {
        public bool CopyMedia { get; set; }

        public bool CreateRomMap { get; set; }

        public bool CreateGenres { get; set; }

        public bool CreateSymbolicLinks { get; internal set; }

        public string MultiSystemName { get; set; }

        public string SettingsTemplateFile { get; set; }        
    }
}
