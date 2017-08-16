﻿using Horsesoft.Frontends.Helper.Model;
using Horsesoft.Frontends.Helper.Model.Hyperspin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Auditing
{
    public interface IHyperspinAudit
    {
        Task<IEnumerable<IFile>> GetUnusedMediaFilesAsync(IEnumerable<Game> gamesList, HsMediaType hsMediaType);

        Task<bool> ScanForMediaAsync(IEnumerable<Game> gamesList);

        Task<bool> ScanMainMenuMediaAsync(IEnumerable<Game> gamesList);
    }
}
