namespace Horsesoft.Frontends.Helper.Model.Hyperspin.Settings
{
    /// <summary>
    /// Hyperspins ini exeinfo
    /// </summary>
    public class ExeInfo
    {
        public string Path { get; set; }
        public string Exe { get; set; }
        public bool UseRomPath { get; set; }
        public string RomPath { get; set; }
        public string RomExtension { get; set; }
        public string Parameters { get; set; }
        public bool SearchSubFolders { get; set; }
        public bool Per_Game_Modules { get; set; }
        public bool PcGame { get; set; }
        public string WinState { get; set; }
        public bool HyperLaunch { get; set; }
        public bool MultiGame_Enabled { get; set; }
    }
}
