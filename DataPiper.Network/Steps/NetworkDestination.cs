using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    internal class NetworkDestination : Destination
    {
        //constructors
        public NetworkDestination(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new NetworkDestinationOptions Options { get => (NetworkDestinationOptions)base.Options; set => base.Options = value; }

        //methods
        protected override void Load(IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                var copyToPath = Path.Combine(Options.FilePath, file.Name);
                LogService.LogDebug($"Moving decrypted file to {copyToPath}");
                file.MoveTo(copyToPath, Options.OverwriteExisting);
            }
        }
    }
}
