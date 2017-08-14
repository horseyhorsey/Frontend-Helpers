using Horsesoft.Frontends.Helper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Tools
{
    public interface IPinTableParse
    {
        string[] GetTableInfo(string tableFile, TableInfo infoType);
    }
}
