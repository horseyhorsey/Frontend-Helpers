using System.Collections.Generic;
using System.Linq;

namespace Frontends.Models.RocketLauncher
{
    public class MediaScanResult
    {
        private string _scanFolderName;

        public IList<string> MatchedFolders { get; set; }
        public IList<string> UnMatchedFolders { get; set; }

        public MediaScanResult(string scanPath)
        {
            _scanFolderName = scanPath;
            MatchedFolders = new List<string>();
            UnMatchedFolders = new List<string>();
        }
    }
}
