using Frontends.Models;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Horsesoft.Frontends.Helper.Tools
{
    public static class SymbolicLinker
    {
        [DllImport("kernel32.dll")]
        public static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLinkType dwFlags);

        public static string SymbolicLinkName { get; set; }
        public static string FileName { get; set; }
        public static SymbolicLinkType SymLinkType { get; set; }

        public static bool CheckThenCreate(string FileToLink, string tempSymlinkFile)
        {
            if (File.Exists(FileToLink))
            {
                SymbolicLinkName = tempSymlinkFile;
                FileName = FileToLink;
                SymLinkType = SymbolicLinkType.File;
                return CreateSymbolicLink(SymbolicLinkName, FileName, SymLinkType);
            }
            else
                return false;
        }

        public static void CreateDirectory(string tempSymlinkFile)
        {
            if (!Directory.Exists(tempSymlinkFile))
                Directory.CreateDirectory(tempSymlinkFile);
        }

    }

    public class SymbolicLinkerException : Exception
    {
        public SymbolicLinkerException(string message) : base(message)
        {            

        }
    }
}
