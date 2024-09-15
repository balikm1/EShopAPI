using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EShop.API.Swagger.Configurators
{
    /// <summary>Represents abstract class for configuring swagger.</summary>
    public abstract class BaseSwaggerConfigurator
    {
        /// <summary>Generates swagger documentation(s).</summary>
        /// <param name="options">Instance of <see cref="SwaggerGenOptions"/>.</param>
        public abstract void GenerateDocuments(SwaggerGenOptions options);

        /// <summary>Configures swagger UI endpoints.</summary>
        /// <param name="options">Instance of <see cref="SwaggerUIOptions"/>.</param>
        public abstract void ConfigureEndpoints(SwaggerUIOptions options);

        /// <summary>Gets application version.</summary>
        protected static string ApplicationVersion
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>Generates api version.</summary>
        /// <param name="options">Instance of <see cref="SwaggerOptions"/>.</param>
        /// <returns>Created instance of <see cref="OpenApiInfo"/>.</returns>
        protected static OpenApiInfo GenerateApiInformation(SwaggerOptions options, string version = null)
        {
            return new OpenApiInfo()
            {
                Title = $"{options.ApplicationName}",
                Version = version != null ? version : ApplicationVersion,
                Description = $"{options.ApplicationDescription}",
                Contact = new OpenApiContact()
                {
                    Name = options.ContactName,
                    Email = options.ContactEmail
                }
            };
        }
    }
}
