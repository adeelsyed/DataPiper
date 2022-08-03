
using System.IO;

namespace DataPiper
{
    public class FileSystemService : IFileSystemService
    {
        public string GetDefaultWorkingDirectory()
        {
            var dir = Path.Combine(Path.GetTempPath(), "DataPiper");
            Directory.CreateDirectory(dir);
            return dir;
        }
    }
}
