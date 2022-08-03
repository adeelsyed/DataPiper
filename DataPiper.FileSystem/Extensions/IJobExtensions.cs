namespace DataPiper
{
    /// <summary>
    /// Allows core package to inject instances from this package without a dependency on this package
    /// </summary>
    public static class IJobExtensions
    {
        /// <summary>
        /// Sets the source
        /// </summary>
        public static void SetSource(this IJob job, FileSystemSourceOptions options)
        {
            var source = (FileSystemSource)job.ServiceProvider.GetService(typeof(FileSystemSource));
            source.Options = options;
            job.SetSource(source);
        }
        /// <summary>
        /// Sets the destination
        /// </summary>
        public static void SetDestination(this IJob job, FileSystemDestinationOptions options)
        {
            var destination = (FileSystemDestination)job.ServiceProvider.GetService(typeof(FileSystemDestination));
            destination.Options = options;
            job.SetDestination(destination);
        }
    }
}
