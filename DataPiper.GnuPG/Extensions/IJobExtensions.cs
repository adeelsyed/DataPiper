using System.Linq;

namespace DataPiper
{
    /// <summary>
    /// Allows core package to inject instances from this package without a dependency on this package
    /// </summary>
    public static class IJobExtensions
    {
        /// <summary>
        /// Adds a transformer to the pipeline
        /// </summary>
        public static Transformer AddTransformer(this IJob job, GnuPGDecryptionTransformerOptions options)
        {
            var transformer = (GnuPGDecryptionTransformer)job.ServiceProvider.GetService(typeof(GnuPGDecryptionTransformer));
            transformer.Options = options;
            return job.AddTransformer(transformer);
        }
        /// <summary>
        /// Adds a transformer to the pipeline
        /// </summary>
        public static Transformer AddTransformer(this IJob job, GnuPGEncryptionTransformerOptions options)
        {
            var transformer = (GnuPGEncryptionTransformer)job.ServiceProvider.GetService(typeof(GnuPGEncryptionTransformer));
            transformer.Options = options;
            return job.AddTransformer(transformer);
        }
    }
}
