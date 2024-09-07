using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EShop.API.Swagger
{
    /// <summary>Represents default swagger configuration setup.</summary>
    public class DefaultSwaggerSetup : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly SwaggerOptions swaggerOptions;

        /// <summary>Initializes new instance of <see cref="DefaultSwaggerSetup"/>.</summary>
        /// <param name="options">Instance of <see cref="IOptions{SwaggerOptions}"/>.</param>
        public DefaultSwaggerSetup(IOptions<SwaggerOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            this.swaggerOptions = options.Value;
        }

        /// <summary>Invoked to configure a <typeparamref name="TOptions"/> instance.</summary>
        /// <param name="options">The options instance to configure.</param>
        public virtual void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc($"v1", GenerateApiInformation(swaggerOptions));
        }

        /// <summary>Generates api version.</summary>
        /// <param name="options">Instance of <see cref="SwaggerOptions"/>.</param>
        /// <returns>Created instance of <see cref="OpenApiInfo"/>.</returns>
        protected static OpenApiInfo GenerateApiInformation(SwaggerOptions options)
        {
            return new OpenApiInfo()
            {
                Title = $"{options.ApplicationName}",
                Version = "v1",
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
