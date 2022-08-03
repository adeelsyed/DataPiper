using System;

namespace DataPiper
{
    public abstract class Step
    {
        public Step(IServiceProvider svcProvider)
        {
            LogService = (ILogService)svcProvider.GetService(typeof(ILogService));
            WorkingDirectory = ((IFileSystemService)svcProvider.GetService(typeof(IFileSystemService))).GetDefaultWorkingDirectory();
        }

        public Options Options { get; set; }
        protected internal ILogService LogService { get; set; }
        protected internal string WorkingDirectory { get; set; }
    }
}
