using Microsoft.Extensions.DependencyInjection;

namespace DataPiper
{
    /// <summary>
    /// Allows client to register services required by this package and configure Options
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers core DataPiper services with the service collection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataPiper(this IServiceCollection services)
        {
            services.AddTransient<IJob, Job>();
            services.AddSingleton<ICmdShellService, CmdShellService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<ILogService, LogService>();
            return services;
        }
    }
}
