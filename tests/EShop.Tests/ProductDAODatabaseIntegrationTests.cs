using EShop.Core.Models;
using EShop.Core.Persistence;
using EShop.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EShop.Tests
{
    [TestFixture]
    public class ProductDAODatabaseIntegrationTests : ProductDAODatabaseTestsBase
    {
        private IDbContextTransaction transaction;

        [SetUp]
        public override void Setup()
        {
            var options = new DbContextOptionsBuilder<EShopDbContext>()
                .UseSqlServer("Server=localhost,1433;Database=EShopDB;User Id=sa;Password=Strong@Passw0rd;Integrated Security=False;Encrypt=False")
                .Options;

            dbContext = new EShopDbContext(options);

            // Start a new transaction
            transaction = dbContext.Database.BeginTransaction();

            dbContext.Database.EnsureCreated();

            productDAO = new ProductDAO(dbContext);

            existingProductCount = dbContext.Products.Count();
        }

        [TearDown]
        public override void Cleanup()
        {
            // Rollback the transaction to avoid modifying real data
            transaction.Rollback();
            transaction.Dispose();

            // Reset identity
            dbContext.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('dbo.Product', RESEED, {existingProductCount})");

            dbContext.Dispose();
        }

        protected override IQueryable<Product> GetTestProducts()
        {
            return new List<Product>
            {
                new Product { Name = "Product1", Price = 10.0m, Description = "Description1", ImgUri = "Uri1" },
                new Product { Name = "Product2", Price = 20.0m, Description = "Description2", ImgUri = "Uri2" }
            }.AsQueryable();
        }

        protected override Product SetupNewProductToAdd()
        {
            newProduct = new Product { Name = "Product3", Price = 30.0m, Description = "Description3", ImgUri = "Uri3" };
            SetupProducts();

            return newProduct;
        }

        protected override Product SetupExistingProductToUpdate()
        {
            SetupProducts();
            updatedProduct = new Product { Id = existingProductCount + 1, Name = "UpdatedProduct", Price = 15.0m, Description = "UpdatedDescription", ImgUri = "UpdatedUri" };

            return updatedProduct;
        }
    }
}
