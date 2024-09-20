using EShop.Core.Models;
using EShop.Core.Persistence;

namespace EShop.Tests
{
    public abstract class ProductDAODatabaseTestsBase : ProductDAOTestsBase
    {
        protected EShopDbContext dbContext;

        protected override void SetupProducts()
        {
            var products = GetTestProducts();

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        protected override void SetupProductToGetById()
        {
            SetupProducts();
        }

        protected Product newProduct;

        protected override Product SetupNewProductToAdd()
        {
            newProduct = new Product { Id = 3, Name = "Product3", Price = 30.0m, Description = "Description3", ImgUri = "Uri3" };
            SetupProducts();

            return newProduct;
        }

        protected override void AssertNewProductAdded()
        {
            Assert.That(dbContext.Products.Count() == existingProductCount + 3);
            Product? existingProduct = dbContext.Products.Find(newProduct?.Id);
            Assert.AreEqual(newProduct?.Name, existingProduct.Name);
            Assert.AreEqual(newProduct?.Price, existingProduct.Price);
            Assert.AreEqual(newProduct?.ImgUri, existingProduct.ImgUri);
            Assert.AreEqual(newProduct?.Description, existingProduct.Description);
        }

        protected Product updatedProduct;

        protected override Product SetupExistingProductToUpdate()
        {
            SetupProducts();
            updatedProduct = new Product { Id = 1, Name = "UpdatedProduct", Price = 15.0m, Description = "UpdatedDescription", ImgUri = "UpdatedUri" };

            return updatedProduct;
        }

        protected override void AssertExistingProductUpdated()
        {
            Product? existingProduct = dbContext.Products.Find(existingProductCount + 1);
            Assert.AreEqual(updatedProduct?.Name, existingProduct.Name);
            Assert.AreEqual(updatedProduct?.Price, existingProduct.Price);
            Assert.AreEqual(updatedProduct?.ImgUri, existingProduct.ImgUri);
            Assert.AreEqual(updatedProduct?.Description, existingProduct.Description);
        }
    }
}
