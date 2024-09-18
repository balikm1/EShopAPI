using EShop.Core.Interfaces;
using EShop.Core.Models;
using EShop.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EShop.Core.Repositories
{
    /// <summary>Implementation of repository for storing products.</summary>
    public class ProductDAO : IProductDAO
    {
        private readonly EShopDbContext dbContext;

        /// <summary>Initializes new instance of <see cref="ProductDAO"/>.</summary>
        /// <param name="dbContext">Instance of <see cref="EShopDbContext"/></param>
        public ProductDAO(EShopDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await dbContext.Products.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize)
        {
            page = page < 1 ? 1 : page; // ensure page is at least 1

            int totalCount = await dbContext.Products.CountAsync();

            var products = await dbContext.Products
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return products;
        }

        /// <inheritdoc />
        public async Task<int> GetAllProductsCount()
        {
            int totalCount = await dbContext.Products.CountAsync();

            return totalCount;
        }

        /// <inheritdoc />
        public async Task<Product?> GetProductById(int id)
        {
            return await dbContext.Products.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task UpdateProduct(Product product)
        {
            var existingProduct = dbContext.Products.Find(product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.ImgUri = product.ImgUri;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;

                await dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task SaveProduct(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
