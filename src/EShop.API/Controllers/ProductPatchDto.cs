namespace EShop.API.Controllers
{
    /// <summary>Represents DTO object for transfering partial product data.</summary>
    public class ProductPatchDto
    {
        /// <summary>Gets or sets name of the product.</summary>
        public string? Name { get; set; }

        /// <summary>Gets or sets URI of an product image.</summary>
        public string? ImgUri { get; set; }

        /// <summary>Gets or sets price of the product.</summary>
        public decimal? Price { get; set; }

        /// <summary>Gets or sets description of the product.</summary>
        public string? Description { get; set; }

        /// <inheritdoc />
        public override string? ToString()
        {
            return $"Product Name: {Name} ImgUri: {ImgUri} Price: {Price} Description: {Description}";
        }
    }
}
