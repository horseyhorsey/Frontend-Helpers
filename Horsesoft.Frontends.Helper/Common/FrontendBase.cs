using Horsesoft.Frontends.Helper.Common.Attributes;
using Horsesoft.Frontends.Models;
using Horsesoft.Frontends.Helper.Paths.Hyperspin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Common
{
    public abstract class FrontendBase : IFrontend
    {
        #region Constructor
        public FrontendBase()
        {
        } 
        #endregion

        /// <summary>
        /// WHere the frontend is installed at
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The front ends exe.
        /// </summary>
        public string Executable { get; set; }

        /// <summary>
        /// Does the frontend have a settings file. Like an ini
        /// </summary>
        public bool HasSettingsFile { get; set; }

        /// <summary>
        /// Gets or sets the media path of the frontend
        /// </summary>
        public string MediaPath { get; set; }

        public FrontendType GetFrontendType()
        {
            Type type = GetType();
            var attr = (FrontendAttribute)type.GetCustomAttribute(typeof(FrontendAttribute));           
            return attr.FrontendType;
        }

        public virtual Task<IEnumerable<string>> GetDatabaseFilesForSystemAsync(string systemName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Launch the frontend
        /// </summary>
        /// <returns></returns>
        public virtual bool Launch()
        {
            return false;
        }

        #region Support Methods
        protected internal IEnumerable<string> GetDatabaseFilesForSystem(string systemName, string dbFileExtFilter = "*.xml")
        {
            //Get db path for system
            var dbPath = System.IO.Path.Combine(Path, Root.Databases, systemName);
            if (!System.IO.Directory.Exists(dbPath))
                throw new System.IO.DirectoryNotFoundException();

            var xmlsInDirectory = new List<string>();

            //Get all files with filet in dir
            var dbFiles = System.IO.Directory.GetFileSystemEntries(dbPath, dbFileExtFilter);

            //Only add databases that contain the system name. *Genre dbs are kept here, we dont want to pick these up.
            foreach (var db in dbFiles)
            {
                var dbName = System.IO.Path.GetFileNameWithoutExtension(db);

                if (dbName.Contains(systemName))
                {
                    xmlsInDirectory.Add(db);
                }
            }

            return xmlsInDirectory.AsEnumerable();
        }
        #endregion
    }
}
