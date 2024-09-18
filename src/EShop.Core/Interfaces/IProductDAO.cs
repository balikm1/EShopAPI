using EShop.Core.Models;

namespace EShop.Core.Interfaces
{
    /// <summary>Represents repository for storing products.</summary>
    public interface IProductDAO
    {
        /// <summary>Retrieves all products from the repository.</summary>
        /// <returns>Enumerator of all products.</returns>
        Task<IEnumerable<Product>> GetAllProducts();

        /// <summary>Retrieves products from the repository with pagination.</summary>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of items on one page.</param>
        /// <returns>Enumerator of products for the current page.</returns>
        public Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize);

        /// <summary>Retrieves total count of all products.</summary>
        /// <returns>Count of products.</returns>
        public Task<int> GetAllProductsCount();

        /// <summary>Retrieves a product by given ID.</summary>
        /// <param name="id">Identifier of the product.</param>
        /// <returns>Product corresponding to the ID or NULL if product is not found.</returns>
        Task<Product?> GetProductById(int id);

        /// <summary>Updates an existing product with new data.</summary>
        /// <param name="product">Product to update.</param>
        Task UpdateProduct(Product product);

        /// <summary>Saves a new product to the repository.</summary>
        /// <param name="product">Product to save.</param>
        Task SaveProduct(Product product);
    }
}
