using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataPiper;

namespace TestClient
{
    class Program
    {
        static void Main()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build()
                .Services.GetService<Test>().Run(); //run startup class
        }
        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var config = hostContext.Configuration;

            //data piper
            services.AddDataPiper();
            services.AddRestApiSource(config.GetSection("MyRestApi"));
            services.AddGnuPGEncryptionTransformer(config.GetSection("GnuPGSettings"));
            services.AddFileSystemDestination(config.GetSection("MyFilePath"));

            //register startup class
            services.AddSingleton<Test>();
        }
    }
}
