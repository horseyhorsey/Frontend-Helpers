using Frontends.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Common
{
    public interface IFrontend
    {
        /// <summary>
        /// Gets or sets the executable for the frontend
        /// </summary>
        string Executable { get; set; }

        /// <summary>
        /// Gets the type of the frontend.
        /// </summary>
        FrontendType GetFrontendType();

        /// <summary>
        /// Gets the database files for system.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName);

        /// <summary>
        /// Gets or sets the path of the frontend
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets or sets the media path of the frontend
        /// </summary>
        string MediaPath { get; set; }

        /// <summary>
        /// Launches the frontend
        /// </summary>
        bool Launch();
    }
}
