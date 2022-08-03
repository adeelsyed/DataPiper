using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    public abstract class Destination : Step
    {
        public Destination(IServiceProvider svcProvider) : base(svcProvider) { }


        //called by Job.Load
        internal void BaseLoad(IEnumerable<FileInfo> files)
        {
            LogService.LogDebug("Starting Load");
            Load(files);
            LogService.LogInfo("Completed Load");
        }

        //called by this.BaseLoad. Implemented by derived classes in extension packages
        /// <summary>
        /// Uses the configured Transformer.Options to load files to the Destination
        /// </summary>
        protected abstract void Load(IEnumerable<FileInfo> files);
    }
}
