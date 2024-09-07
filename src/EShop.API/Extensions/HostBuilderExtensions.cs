using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EShop.API.Extensions
{
    /// <summary>Represents extension class for <see cref="IHostBuilder"/>.</summary>
    public static class HostBuilderExtensions
    {
        /// <summary>Configures default configuration providers.</summary>
        /// <param name="builder">Instance of <see cref="IHostBuilder"/>.</param>
        /// <returns>Modified instance of <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureDefaultConfigurationProviders(this IHostBuilder builder)
        {
            return builder
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);

                    // Adding YML configuration builder.
                    config.AddYamlFile("appsettings.yml", optional: true);
                    config.AddYamlFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.yml", optional: true);

                    // Adding Environment variables configuration builder.
                    config.AddEnvironmentVariables();
                });
        }
    }
}
