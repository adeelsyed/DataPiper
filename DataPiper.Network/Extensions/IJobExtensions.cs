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
        public static void SetSource(this IJob job, NetworkSourceOptions options)
        {
            var source = (NetworkSource)job.ServiceProvider.GetService(typeof(NetworkSource));
            source.Options = options;
            job.SetSource(source);
        }
        /// <summary>
        /// Sets the destination
        /// </summary>
        public static void SetDestination(this IJob job, NetworkDestinationOptions options)
        {
            var destination = (NetworkDestination)job.ServiceProvider.GetService(typeof(NetworkDestination));
            destination.Options = options;
            job.SetDestination(destination);
        }
    }
}
