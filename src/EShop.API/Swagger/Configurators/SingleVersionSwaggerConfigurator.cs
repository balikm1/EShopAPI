using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EShop.API.Swagger.Configurators
{
    /// <summary>Represents extension of <see cref="BaseSwaggerConfigurator"/> for single swagger api.</summary>
    public class SingleVersionSwaggerConfigurator : BaseSwaggerConfigurator
    {
        private readonly SwaggerOptions swaggerOptions;

        /// <summary>Initializes new instance of <see cref="SingleVersionSwaggerConfigurator"/>.</summary>
        /// <param name="options">Instance of <see cref="IOptions{SwaggerOptions}"/>.</param>
        public SingleVersionSwaggerConfigurator(IOptions<SwaggerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.swaggerOptions = options.Value;
        }

        /// <summary>Generates swagger documentation(s).</summary>
        /// <param name="options">Instance of <see cref="SwaggerGenOptions"/>.</param>
        public override void GenerateDocuments(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", GenerateApiInformation(swaggerOptions));
        }

        /// <summary>Configures swagger UI endpoints.</summary>
        /// <param name="options">Instance of <see cref="SwaggerUIOptions"/>.</param>
        public override void ConfigureEndpoints(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        }
    }
}
