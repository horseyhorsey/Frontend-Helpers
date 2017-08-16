using System.IO;

namespace Frontends.Models.Hyperspin
{
    public class HyperspinFile : IFile
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }

        public HyperspinFile(string fileName)
        {
            FullPath = fileName;
            FileName = Path.GetFileNameWithoutExtension(fileName);
            Extension = Path.GetExtension(fileName);
        }
    }
}
