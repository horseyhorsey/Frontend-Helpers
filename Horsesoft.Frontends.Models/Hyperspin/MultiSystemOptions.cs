namespace Frontends.Models.Hyperspin
{
    /// <summary>
    /// Options for creating a Hyperspin multi system
    /// </summary>
    public class MultiSystemOptions
    {
        public bool CopyMedia { get; set; }

        public bool CreateRomMap { get; set; }

        public bool CreateGenres { get; set; }

        public bool CreateSymbolicLinks { get; set; }

        public string MultiSystemName { get; set; }

        public string SettingsTemplateFile { get; set; }        
    }
}
