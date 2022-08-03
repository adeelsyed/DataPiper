using System;
using System.Collections.Generic;
using System.IO;

namespace DataPiper
{
    public abstract class Transformer : Step
    {
        private readonly IEventService _eventService;

        //constructors
        public Transformer(IServiceProvider svcProvider) : base(svcProvider) 
        {
            _eventService = (IEventService)svcProvider.GetService(typeof(IEventService));
        }

        //events
        /// <summary>
        /// Occurs after each transformation
        /// </summary>
        public event EventHandler<FilesEventArgs> OnTransformed;

        //methods
        //called by Job.Transform
        internal IEnumerable<FileInfo> BaseTransform(IEnumerable<FileInfo> files)
        {
            LogService.LogDebug("Starting Transform");
            files = Transform(files);
            LogService.LogInfo("Completed Transform: {files}", files.ToCsv());

            _eventService.Invoke(OnTransformed, files, nameof(OnTransformed));

            return files;
        }
        //called by this.BaseTransform. Implemented by derived classes in extension packages
        /// <summary>
        /// Uses configured Transformer.Options to process the files and pass them on to the next step in the Transformers pipeline
        /// </summary>
        protected abstract IEnumerable<FileInfo> Transform(IEnumerable<FileInfo> files);
    }
}
