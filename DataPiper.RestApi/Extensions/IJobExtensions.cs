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
        public static void SetSource(this IJob job, RestApiSourceOptions options)
        {
            var source = (RestApiSource)job.ServiceProvider.GetService(typeof(RestApiSource));
            source.Options = options;
            job.SetSource(source);
        }
    }
}
