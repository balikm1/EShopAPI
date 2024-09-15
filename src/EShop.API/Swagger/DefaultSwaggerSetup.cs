using System;
using EShop.API.Swagger.Configurators;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EShop.API.Swagger
{
    /// <summary>Represents default swagger configuration setup.</summary>
    public class DefaultSwaggerSetup : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly SwaggerConfiguratorFactory swaggerConfiguratorFactory;

        /// <summary>Initializes new instance of <see cref="DefaultSwaggerSetup"/>.</summary>
        /// <param name="swaggerConfiguratorFactory">Instance of <see cref="SwaggerConfiguratorFactory"/>.</param>
        public DefaultSwaggerSetup(SwaggerConfiguratorFactory swaggerConfiguratorFactory)
        {
            this.swaggerConfiguratorFactory = swaggerConfiguratorFactory ?? throw new ArgumentNullException(nameof(swaggerConfiguratorFactory));
        }

        /// <summary>Invoked to configure a <typeparamref name="TOptions"/> instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(SwaggerGenOptions options)
        {
            swaggerConfiguratorFactory.Create().GenerateDocuments(options);
        }
    }
}
