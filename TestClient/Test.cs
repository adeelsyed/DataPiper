using DataPiper;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;

namespace TestClient
{
    public class Test
    {
        private readonly IJob _job;
        private readonly RestApiSourceOptions _restApiOptions;
        private readonly GnuPGEncryptionTransformerOptions _gnupgOptions;
        private readonly FileSystemDestinationOptions _fileSystemSettings;

        public Test(IJob job, 
            IOptions<RestApiSourceOptions> restApiOptions, 
            IOptions<GnuPGEncryptionTransformerOptions> gnupgOptions, 
            IOptions<FileSystemDestinationOptions> fileSystemSettings)
        { 
            _job = job;
            _restApiOptions = restApiOptions.Value;
            _gnupgOptions = gnupgOptions.Value;
            _fileSystemSettings = fileSystemSettings.Value;
        }

        public void Run()
        {
            _job.SetSource(_restApiOptions);
            _job.AddTransformer(_gnupgOptions);
            _job.SetDestination(_fileSystemSettings);
            _job.Run();
        }

        private void ChangeFilename(object sender, FilesEventArgs e)
        {
            var timestamp = DateTime.Now.ToString("s");
            var file = e.Files.Single();
            file.MoveTo(Path.Combine(file.DirectoryName, timestamp + file.Extension));
        }
    }
}
