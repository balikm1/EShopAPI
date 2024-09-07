using System;
using EShop.API.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace EShop.API
{
    /// <summary>Represents startup operations such as configuration settings, dependency registration and configuring web api pipeline.</summary>
    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup"/> class.</summary>
        /// <param name="configuration">Configuration accessor</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>Gets configuration accessor.</summary>
        public IConfiguration Configuration { get; }

        /// <summary>This method gets called by the runtime. Use this method to add services to the container.</summary>
        /// <param name="services">Instance of Microsoft DI IoC container</param>
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger.Debug("Injecting dependencies for starting application.");

            services.AddControllers();

            services.Configure<SwaggerOptions>(Configuration.GetSection("Dotnet:Swagger"), opt => opt.BindNonPublicProperties = true);
            services.AddSwaggerGen();
            services.ConfigureOptions<DefaultSwaggerSetup>();

            Log.Logger.Debug("Dependencies are injected for starting application");
        }

        /// <summary>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</summary>
        /// <param name="app">Application builder instance.</param>
        /// <param name="env">Hosting environment instance.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            Log.Logger.Debug("Application configuration is started.");

            try
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });

                app.UseHttpsRedirection();
                app.UseRouting();

                //app.UseCors();

                //app.UseAuthentication();
                //app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                Log.Logger.Debug("Application configuration is completed.");
            }
            catch (OptionsValidationException ex)
            {
                Log.Logger.Error($"Exception occurred due to misconfiguration of {ex.OptionsType}. Details: {string.Join(",", ex.Failures)}", ex);
                throw;
            }
        }
    }
}
