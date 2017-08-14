namespace Horsesoft.Frontends.Helper.Model
{
    public interface IFile
    {
        string Extension { get; set; }

        string FileName { get; set; }

        string FullPath { get; set; }        
    }
}