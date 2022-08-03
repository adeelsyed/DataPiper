using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
    public abstract class Source : Step
    {
        public Source(IServiceProvider svcProvider) : base(svcProvider) { }

        //called by Job.Extract
        internal IEnumerable<FileInfo> BaseExtract()
        {
            LogService.LogDebug("Starting Extract");
            
            var extractedFiles = Extract();

            ValidateExtractedFiles(extractedFiles);

            LogService.LogInfo("Completed Extract: {extractedFiles}", extractedFiles.ToCsv());

            return extractedFiles;
        }

        //called by this.BaseExtract. Implemented by derived classes in extension packages
        /// <summary>
        /// Uses the settings in Source.Options to copy data to the WorkingDirectory as one or more files.
        /// </summary>
        protected abstract IEnumerable<FileInfo> Extract();

        private void ValidateExtractedFiles(IEnumerable<FileInfo> extractedFiles)
        {
            if (!extractedFiles.All(f => IsSameDirectory(f.FullName, WorkingDirectory)))
                throw new Exception($"One or more files are not stored in the WorkingDirectory. Extracted files: {extractedFiles.ToCsv()}");
        }
        private bool IsSameDirectory(string dir1, string dir2)
        {
            return 0 == string.Compare(
                Path.GetFullPath(dir1).TrimEnd('\\'),
                Path.GetFullPath(dir2).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
