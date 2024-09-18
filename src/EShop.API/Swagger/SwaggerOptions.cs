namespace EShop.API.Swagger
{
    /// <summary>Represents class for defining swagger options.</summary>
    public class SwaggerOptions
    {
        /// <summary>Gets application name.</summary>
        public string ApplicationName { get; private set; } = string.Empty;

        /// <summary>Gets application description.</summary>
        public string ApplicationDescription { get; private set; } = string.Empty;

        /// <summary>Gets contact name.</summary>
        public string ContactName { get; private set; } = string.Empty;

        /// <summary>Gets contact email.</summary>
        public string ContactEmail { get; private set; } = string.Empty;
    }
}
