using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
	internal class NetworkSource : Source
	{
		//constructors
		public NetworkSource(IServiceProvider svcProvider) : base(svcProvider) { }

		//properties
		private new NetworkSourceOptions Options { get => (NetworkSourceOptions)base.Options; set => base.Options = value; }

		//methods
		protected override IEnumerable<FileInfo> Extract()
		{
			var extractedFiles = new List<FileInfo>();

			//check network directory
			var networkFiles = new DirectoryInfo(Options.FilePath).GetFiles();
			if (networkFiles.Count() == 0)
			{
				LogService.LogInfo("No files found on Network server");
				return extractedFiles; //empty
			}
			LogService.LogDebug("All files on Network server: {networkFiles}", networkFiles.ToCsv());

			//download files
			foreach (var networkFile in networkFiles)
			{
				LogService.LogDebug($"Extracting {networkFile} to {WorkingDirectory}");
				var copiedFile = networkFile.CopyTo(WorkingDirectory, overwrite: true);
				extractedFiles.Add(copiedFile);
			}
			return extractedFiles;
		}
    }
}
