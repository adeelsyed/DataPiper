using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
    internal class FileSystemSource : Source
    {
        //constructors
        public FileSystemSource(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new FileSystemSourceOptions Options { get => (FileSystemSourceOptions)base.Options; set => base.Options = value; }

        //methods
        protected override IEnumerable<FileInfo> Extract()
        {
            var extractedFiles = new List<FileInfo>();

            //check directory
            var files = new DirectoryInfo(Options.Path).GetFiles();
            if(files.Count() == 0)
            {
                LogService.LogInfo("No files found at file path");
                return extractedFiles;
            }
            LogService.LogDebug("All files in specified directory: {files}", files.ToCsv());

            //copy files
            foreach (var file in files)
            {
                LogService.LogDebug("Copying {file} to {WorkingDirectory}", file.Name, WorkingDirectory);
                var copiedFile = file.CopyTo(WorkingDirectory, overwrite: true);
                extractedFiles.Add(copiedFile);
            }

            return extractedFiles;
        }
    }
}
