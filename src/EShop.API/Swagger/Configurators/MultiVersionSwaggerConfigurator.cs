using System;
using System.Linq;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EShop.API.Swagger.Configurators
{
    /// <summary>Represents extension of <see cref="BaseSwaggerConfigurator"/> for configuring multi api versions.</summary>
    public class MultiVersionSwaggerConfigurator : BaseSwaggerConfigurator
    {
        private readonly MultiVersionSwaggerOptions swaggerOptions;
        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

        /// <summary>Initializes new instance of <see cref="MultiVersionSwaggerConfigurator"/>.</summary>
        /// <param name="options">Instance of <see cref="IOptions{SwaggerOptions}"/>.</param>
        /// <param name="apiVersionDescriptionProvider">Instance of <see cref="IApiVersionDescriptionProvider"/>.</param>
        public MultiVersionSwaggerConfigurator(
            IOptions<MultiVersionSwaggerOptions> options,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
            this.swaggerOptions = options.Value;
        }

        /// <summary>Generates swagger documentation(s).</summary>
        /// <param name="options">Instance of <see cref="SwaggerGenOptions"/>.</param>
        public override void GenerateDocuments(SwaggerGenOptions options)
        {
            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc($"{apiVersionDescription.GroupName}", GenerateApiInformation(swaggerOptions.Versions[$"{apiVersionDescription.GroupName}"], $"{apiVersionDescription.GroupName}"));
            }

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        }

        /// <summary>Configures swagger UI endpoints.</summary>
        /// <param name="options">Instance of <see cref="SwaggerUIOptions"/>.</param>
        public override void ConfigureEndpoints(SwaggerUIOptions options)
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                
                options.SwaggerEndpoint(string.Format($"/swagger/{description.GroupName}/swagger.json"), description.GroupName);
            }
        }
    }
}
