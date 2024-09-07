using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Settings.Configuration;

namespace EShop.API.Extensions
{
    /// <summary>Represents class for extension methods related to logging.</summary>
    public static class LoggingExtensions
    {
        /// <summary>Gets serilog configuration section name.</summary>
        public const string SerilogSection = "Dotnet:Serilog";

        /// <summary>Creates initial logger for logging events before application starts.</summary>
        /// <param name="log">Instance of <see cref="ILogger"/>.</param>
        public static void CreateInitialLogger(this ILogger log)
        {
            Log.Logger = new LoggerConfiguration()
                .ConfigureDefaults()
                .CreateBootstrapLogger();
        }

        /// <summary>Configures default logging setup.</summary>
        /// <param name="builder">Instance of <see cref="IHostBuilder"/>.</param>
        /// <returns>Modified instance of <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder UseDefaultSerilogLogger(this IHostBuilder builder)
        {
            builder.UseSerilog((ctx, serviceProvider, loggerCfg) =>
            {
                loggerCfg
                    .ConfigureDefaults()
                    .ReadFrom.Configuration(ctx.Configuration, new ConfigurationReaderOptions() { SectionName = SerilogSection } )
                    .ReadFrom.Services(serviceProvider);
            });

            return builder;
        }

        private static LoggerConfiguration ConfigureDefaults(this LoggerConfiguration configuration)
        {
            return configuration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
                .WriteTo.Console();
        }
    }
}
