using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataPiper
{
    internal class SftpSource : Source
    {
        //constructors
        public SftpSource(IServiceProvider svcProvider) : base(svcProvider) { }

        //properties
        private new SftpSourceOptions Options { get => (SftpSourceOptions)base.Options; set => base.Options = value; }

        //public methods
        protected override IEnumerable<FileInfo> Extract()
        {
            var extractedFiles = new List<FileInfo>();

            ConnectionInfo con = SftpHelper.GetConnectionInfo(Options.Host, Options.UserName, Options.Password, Options.PrivateKeyPath);
            using (var client = new SftpClient(con))
            {
                //connect to SFTP
                LogService.LogDebug($"Connecting to SFTP: {Options.Host}");
                client.HostKeyReceived += (sender, e) => SftpHelper.ValidateHostKey(Options.HostKeyFingerprint, e);
                if (Options.ConnectionTimeout.HasValue)
                    client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(Options.ConnectionTimeout.Value);
                client.Connect();

                //check remote directory
                var remoteFiles = client.ListDirectory(Options.RemotePath).Where(f => !string.IsNullOrEmpty(Path.GetExtension(f.Name))); //files only
                if (remoteFiles.Count() == 0)
                {
                    LogService.LogInfo("No files found on SFTP server");
                    return extractedFiles; //empty
                }
                LogService.LogDebug("All files on SFTP server: {remoteFiles}", string.Join(",", remoteFiles.Select(f => f.Name).ToArray()));

                //filter
                if (!string.IsNullOrWhiteSpace(Options.SearchPattern))
                {
                    LogService.LogDebug($"Filtering to files that match /{Options.SearchPattern}/");
                    remoteFiles = remoteFiles.Where(f => Regex.IsMatch(f.Name, Options.SearchPattern));
                    LogService.LogDebug("Filtered files: {0}", string.Join(",", remoteFiles.Select(f => f.Name).ToArray()));
                }

                //download files
                foreach (var remoteFile in remoteFiles)
                {
                    var localFilePath = Path.Combine(WorkingDirectory, remoteFile.Name);
                    LogService.LogDebug($"Downloading {remoteFile.FullName} to {localFilePath}");
                    using (var localFile = File.Create(localFilePath))
                        client.DownloadFile(remoteFile.FullName, localFile);

                    var file = new FileInfo(localFilePath);
                    extractedFiles.Add(file);
                }
            }

            return extractedFiles;
        }
    }
}
