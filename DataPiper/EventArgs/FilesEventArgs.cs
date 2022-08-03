using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    public class FilesEventArgs : EventArgs
    {
        public IEnumerable<FileInfo> Files { get; set; }

        public FilesEventArgs(IEnumerable<FileInfo> files)
        {
            Files = files;
        }
    }
}
