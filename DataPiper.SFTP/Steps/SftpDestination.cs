using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    internal class SftpDestination : Destination
	{
		//constructors
		public SftpDestination(IServiceProvider svcProvider) : base(svcProvider) { }

		//properties
		private new SftpDestinationOptions Options { get => (SftpDestinationOptions)base.Options; set => base.Options = value; }

		//methods
		protected override void Load(IEnumerable<FileInfo> files)
		{
			ConnectionInfo con = SftpHelper.GetConnectionInfo(Options.Host, Options.UserName, Options.Password, Options.PrivateKeyPath);
			using (var client = new SftpClient(con))
			{
				//connect to SFTP
				LogService.LogDebug($"Connecting to SFTP: {Options.Host}");
				client.HostKeyReceived += (sender, e) => SftpHelper.ValidateHostKey(Options.HostKeyFingerprint, e);
				if (Options.ConnectionTimeout.HasValue)
					client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(Options.ConnectionTimeout.Value);
				client.Connect();

				foreach (var file in files)
				{
					var copyToPath = $"{Options.RemotePath}/{file.Name}";

					using (var uplfileStream = File.OpenRead(file.FullName))
					{
						client.UploadFile(uplfileStream, copyToPath, true);
					}
					LogService.LogDebug($"Moving decrypted file to {copyToPath}");
					file.Delete();
				}
				client.Disconnect();
			}
		}
	}
}
