using System.Collections.Generic;
using Frontends.Models.Hyperspin;
using Frontends.Models.Interfaces;
using Frontends.Models.RocketLauncher;
using System.IO;
using System;
using System.Linq;

namespace Horsesoft.Frontends.Helper.Media
{
    public class MediaHelperRl : IMediaHelperRl
    {
        public string[] GetAllFolders(string dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            return Directory.GetDirectories(dir);
        }

        public MediaScanResult MatchFoldersToGames(string[] directories, IEnumerable<Game> gamesList)
        {
            if (gamesList.Count() == 0)
                throw new NullReferenceException("No games exist in games list");

            int matchedFolderCount = 0;
            int[] results = new int[4];
            MediaScanResult result = new MediaScanResult("");

            //If a directory matches a game in the list , increment the matchedFolderCount
            foreach (var directory in directories)
            {
                //var dirName = Path.GetFileNameWithoutExtension(directory);
                var dirName = Path.GetFileName(directory);

                if (gamesList.Any(x => x.RomName.ToLower() == dirName.ToLower()))
                {
                    matchedFolderCount++;
                    result.MatchedFolders.Add(dirName);
                }
                else
                {
                    result.UnMatchedFolders.Add(dirName);
                }
            }

            var dirCount = directories.Count();
            results[0] = dirCount;
            results[1] = matchedFolderCount;
            results[2] = gamesList.Count() - matchedFolderCount;
            results[3] = dirCount - matchedFolderCount;

            return result;
        }
    }
}
