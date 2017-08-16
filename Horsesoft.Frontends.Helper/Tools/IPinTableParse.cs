using Frontends.Models;

namespace Horsesoft.Frontends.Helper.Tools
{
    /// <summary>
    /// Visual PInball & Future Pinball include a gameinfo file inside the table file.
    /// </summary>
    public interface IPinTableParse
    {
        string[] GetTableInfo(string tableFile, TableInfo infoType);
    }
}
