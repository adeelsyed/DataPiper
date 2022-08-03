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
        public static void SetSource(this IJob job, SqlClientSourceOptions options)
        {
            var source = (SqlClientSource)job.ServiceProvider.GetService(typeof(SqlClientSource));
            source.Options = options;
            job.SetSource(source);
        }
        /// <summary>
        /// Sets the destination
        /// </summary>
        public static void SetDestination(this IJob job, SqlBulkCopyDestinationOptions options)
        {
            var destination = (SqlBulkCopyDestination)job.ServiceProvider.GetService(typeof(SqlBulkCopyDestination));
            destination.Options = options;
            job.SetDestination(destination);
        }
    }
}
