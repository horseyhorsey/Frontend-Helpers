using System;
using System.IO;

namespace Horsesoft.Frontends.Helper.Tools
{
    public class GameLaunch
    {
        public void RocketLaunchGame(string RlPath, string systemName, string RomName, string HsPath)
        {
            if (Directory.Exists(RlPath))
            {
                try
                {
                    System.Diagnostics.Process.Start(RlPath + "\\Rocketlauncher.exe",
                        "-s " + "\"" + systemName + "\"" + " -r " + "\"" + RomName + "\""
                        + " -f " + HsPath + "\\HyperSpin.exe"
                        + " -p " + "HyperSpin");
                }
                catch (Exception)
                {

                }
            }
        }

        public void RocketLaunchGameWithMode(string RlPath,
            string systemName, string RomName, string mode)
        {
            if (Directory.Exists(RlPath))
            {
                try
                {
                    System.Diagnostics.Process.Start(RlPath +
                        "\\Rocketlauncher.exe",
                        "-s " + "\"" + systemName + "\"" + " -r " + "\"" + RomName + "\"" +
                        //" -f " + HsPath + "\\HyperSpin.exe" +
                        " -m " + mode + " -p hyperspin");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
