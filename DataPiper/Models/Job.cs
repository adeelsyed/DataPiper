using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataPiper
{
    public class Job : IJob
    {
        //fields
        private Source _source;
        private readonly List<Transformer> _transformers;
        private Destination _destination;
        private readonly IEventService _eventService;
        private readonly ILogService _logService;

        //constructors
        public Job(IEventService eventService, ILogService logService, IServiceProvider serviceProvider)
        {
            _transformers = new List<Transformer>();
            _eventService = eventService;
            _logService = logService;
            ServiceProvider = serviceProvider;
        }

        //properties
        //needed by extension packages to create instances of derived BaseSteps
        public IServiceProvider ServiceProvider { get; set; }

        //events
        public event EventHandler<FilesEventArgs> OnExtracted;
        public event EventHandler<FilesEventArgs> OnPreLoad;

        //public methods
        public IJobResult Run()
        {
            try
            {
                //extract
                var extractedFiles = Extract();

                //return if nothing to extract
                if (extractedFiles.Count() == 0)
                    return new JobResult(success: true);

                //transform
                var transformedFiles = Transform(extractedFiles);

                //load
                Load(transformedFiles);

                //cleanup and return
                CleanUp(extractedFiles, transformedFiles);
                return new JobResult(success: true);
            }
            catch (Exception ex)
            {
                return new JobResult(ex);
            }
        }
        public void SetSource(Source source)
        {
            _source = source;
        }
        public Transformer AddTransformer(Transformer transformer)
        {
            _transformers.Add(transformer);
            return transformer;
        }
        public void SetDestination(Destination destination)
        {
            _destination = destination;
        }
        public void SetWorkingDirectory(string path)
        {
            _source.WorkingDirectory = path;
            _transformers.ForEach(t => t.WorkingDirectory = path);
            _destination.WorkingDirectory = path;
        }

        //private methods
        private IEnumerable<FileInfo> Extract()
        {
            //extract
            var extractedFiles =_source.BaseExtract();

            //raise event
            _eventService.Invoke(OnExtracted, extractedFiles, nameof(OnExtracted));

            return extractedFiles;
        }
        private IEnumerable<FileInfo> Transform(IEnumerable<FileInfo> extractedFiles)
        {
            //return if nothing to transform
            if (_transformers.Count() == 0)
                return extractedFiles;

            //run transformers
            IEnumerable<FileInfo> transformedFiles = extractedFiles;
            foreach (var tf in _transformers)
                transformedFiles = tf.BaseTransform(transformedFiles);

            return transformedFiles;
        }
        private void Load(IEnumerable<FileInfo> transformedFiles)
        {
            //raise event
            _eventService.Invoke(OnPreLoad, transformedFiles, nameof(OnPreLoad));

            //load
            _destination.BaseLoad(transformedFiles);
        }
        private void CleanUp(IEnumerable<FileInfo> extractedFiles, IEnumerable<FileInfo> transformedFiles)
        {
            foreach (var file in extractedFiles.Union(transformedFiles))
            {
                try
                {
                    _logService.LogDebug("Deleting working file {file}", file);
                    file.Delete();
                }
                catch(Exception ex)
                {
                    _logService.LogError(ex, "The ETL job completed, but the file '{file}' could not be deleted from the working directory.", file.FullName);
                }
            }
        }
    }
}
