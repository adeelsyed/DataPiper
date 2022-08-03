using System;
using System.Collections.Generic;

namespace DataPiper
{
    public interface IJob
    {
        //properties
        //needed by extension packages to create instances of derived BaseSteps
        IServiceProvider ServiceProvider { get; set; }

        //events
        /// <summary>
        /// Occurs after data has been copied as one or more files to the WorkingDirectory, before any transformations
        /// </summary>
        event EventHandler<FilesEventArgs> OnExtracted;
        /// <summary>
        /// Occurs before data is uploaded to Destination, after all transformations
        /// </summary>
        event EventHandler<FilesEventArgs> OnPreLoad;

        //methods
        /// <summary>
        /// Sets the source of the data
        /// </summary>
        void SetSource(Source source);
        /// <summary>
        /// Adds a transformer to the pipeline
        /// </summary>
        /// <returns></returns>
        Transformer AddTransformer(Transformer transformer);
        /// <summary>
        /// Sets the destination of the data
        /// </summary>
        void SetDestination(Destination destination);
        /// <summary>
        /// Sets the filepath where data files will be staged for transformation and loading. The default is the user's temp directory. 
        /// Once data is successfully loaded to the destination, working files will be deleted.
        /// </summary>
        void SetWorkingDirectory(string path);
        /// <summary>
        /// Runs the job
        /// </summary>
        IJobResult Run();
    }
}
