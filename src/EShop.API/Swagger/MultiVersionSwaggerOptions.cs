using System.Collections.Generic;

namespace EShop.API.Swagger
{
    /// <summary>Represents class for defining multi version swagger options.</summary>
    public class MultiVersionSwaggerOptions
    {
        /// <summary>Gets swagger options for particular API versions.</summary>
        public Dictionary<string, SwaggerOptions> Versions { get; private set; } = new Dictionary<string, SwaggerOptions>();
    }
}
