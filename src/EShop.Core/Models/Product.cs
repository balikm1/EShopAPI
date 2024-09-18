namespace EShop.Core.Models
{
    /// <summary>Represents product and its properties.</summary>
    public class Product
    {
        /// <summary>Gets or sets identifier of the product.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets name of the product.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Gets or sets URI of an product image.</summary>
        public string ImgUri { get; set; } = string.Empty;

        /// <summary>Gets or sets price of the product.</summary>
        public decimal Price { get; set; }

        /// <summary>Gets or sets description of the product.</summary>
        public string? Description { get; set; }

        /// <inheritdoc />
        public override string? ToString()
        {
            return $"Product Id: {Id} Name: {Name} ImgUri: {ImgUri} Price: {Price} Description: {Description}";
        }
    }
}
