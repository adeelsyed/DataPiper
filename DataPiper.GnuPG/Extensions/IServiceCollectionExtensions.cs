using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataPiper
{
    /// <summary>
    /// Allows client to register services required by this package and configure Options
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the DataPiper step and configures its options
        /// </summary>
        /// <param name="configSection">The configuration section from which to load options</param>
        /// <param name="name">If creating more than one instance of the same step with different options, use a name to differentiate them</param>
        public static IServiceCollection AddGnuPGDecryptionTransformer(this IServiceCollection services, IConfiguration configSection, string name = null)
        {
            if (name != null)
                return services.AddTransient<GnuPGDecryptionTransformer>().Configure<GnuPGDecryptionTransformerOptions>(name, configSection);
            return services.AddTransient<GnuPGDecryptionTransformer>().Configure<GnuPGDecryptionTransformerOptions>(configSection);
        }
        /// <summary>
        /// Registers the DataPiper step and configures its options
        /// </summary>
        /// <param name="configSection">The configuration section from which to load options</param>
        /// <param name="name">If creating more than one instance of the same step with different options, use a name to differentiate them</param>
        public static IServiceCollection AddGnuPGEncryptionTransformer(this IServiceCollection services, IConfiguration configSection, string name = null)
        {
            if (name != null)
                return services.AddTransient<GnuPGEncryptionTransformer>().Configure<GnuPGEncryptionTransformerOptions>(name, configSection);
            return services.AddTransient<GnuPGEncryptionTransformer>().Configure<GnuPGEncryptionTransformerOptions>(configSection);
        }
    }
}
