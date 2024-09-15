using System;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EShop.API.Swagger.Configurators
{
    /// <summary>Represents class for creating <see cref="BaseSwaggerConfigurator"/>.</summary>
    public class SwaggerConfiguratorFactory
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>Initializes new instance of <see cref="SwaggerConfiguratorFactory"/>.</summary>
        /// <param name="serviceProvider">Instance of <see cref="IServiceProvider"/>.</param>
        public SwaggerConfiguratorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>Creates corresponding <see cref="BaseSwaggerConfigurator"/>.</summary>
        /// <returns>Found <see cref="BaseSwaggerConfigurator"/>.</returns>
        public BaseSwaggerConfigurator Create()
        {
            // Checking api versioning is registered or not. If yes then we are configuring swagger documentation for multi, else for single.
            var apiVersionDescriptionProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();
            if (apiVersionDescriptionProvider != null)
            {
                return new MultiVersionSwaggerConfigurator(
                    serviceProvider.GetRequiredService<IOptions<MultiVersionSwaggerOptions>>(),
                    apiVersionDescriptionProvider);
            }

            return new SingleVersionSwaggerConfigurator(serviceProvider.GetRequiredService<IOptions<SwaggerOptions>>());
        }
    }
}
