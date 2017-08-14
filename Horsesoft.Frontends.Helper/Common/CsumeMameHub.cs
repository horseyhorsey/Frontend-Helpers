using Horsesoft.Frontends.Helper.Common.Attributes;
using System;
using System.Diagnostics;

namespace Horsesoft.Frontends.Helper.Common
{
    [Frontend(Model.FrontendType.Csume)]
    public class CsumeMameHub : FrontendBase, ICsume
    {
        public override bool Launch()
        {
            Debug.WriteLine("Launching csume");

            return LaunchGame("MAME","horse","carpolo","", 6969, true);
        }

        #region Support Methods
        /// <summary>
        /// Loads the rom.
        /// "CSUME<system> -<media> <gamename> -server|-client -hostname<IP number> -username<name> -port<port>""
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        /// <param name="host">if set to <c>true</c> [host].</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException"></exception>
        private bool LaunchGame(string systemName, string userName, string rom, string ip, int port, bool host)
        {
            if (string.IsNullOrWhiteSpace(Path))
                throw new NullReferenceException();

            var startInfo = new ProcessStartInfo();
            //startInfo.Arguments = $"Mame carpolo -username horse";

            var connectionType = host ? "-server" : "-client";

            startInfo.WorkingDirectory = Path;
            startInfo.Arguments = $"{systemName} {rom} {connectionType} -username {userName}";
            startInfo.UseShellExecute = false;
            startInfo.FileName = "csume64.exe";

            try
            {
                Process.Start(startInfo).WaitForExit();

                return true;
            }
            catch { return false; }
        } 
        #endregion
    }

    public interface ICsume
    {
    }
}
