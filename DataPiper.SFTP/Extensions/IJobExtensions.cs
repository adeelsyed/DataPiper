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
        public static void SetSource(this IJob job, SftpSourceOptions options)
        {
            var source = (SftpSource)job.ServiceProvider.GetService(typeof(SftpSource));
            source.Options = options;
            job.SetSource(source);
        }
        /// <summary>
        /// Sets the destination
        /// </summary>
        public static void SetDestination(this IJob job, SftpDestinationOptions options)
        {
            var destination = (SftpDestination)job.ServiceProvider.GetService(typeof(SftpDestination));
            destination.Options = options;
            job.SetDestination(destination);
        }
    }
}
