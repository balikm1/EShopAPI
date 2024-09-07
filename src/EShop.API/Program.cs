using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using EShop.API.Extensions;
using Serilog;

namespace EShop.API
{
    /// <summary>Represents entry class of application.</summary>
    public class Program
    {
        /// <summary>Entry point of program.</summary>
        /// <param name="args">List of provided arguments.</param>
        public static async Task<int> Main(string[] args)
        {
            Log.Logger.CreateInitialLogger();

            try
            {
                await CreateHostBuilder(args)
                    .Build()
                    .RunAsync();

                Log.Information("Application is shutdown gracefully.");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal error occured while running application.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureDefaultConfigurationProviders()
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                        .UseStartup<Startup>()
                        .CaptureStartupErrors(true);
                })
                .UseDefaultSerilogLogger();
        }
    }
}
