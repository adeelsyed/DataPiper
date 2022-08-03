using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    internal class FileSystemDestination : Destination
    {
        //constructors
        public FileSystemDestination(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new FileSystemDestinationOptions Options { get => (FileSystemDestinationOptions)base.Options; set => base.Options = value; }

        //methods
        protected override void Load(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                var copyToPath = System.IO.Path.Combine(Options.Path, file.Name);
                LogService.LogDebug($"Moving file to {copyToPath}");
                file.MoveTo(copyToPath, overwrite: true);
            }
        }
    }
}
