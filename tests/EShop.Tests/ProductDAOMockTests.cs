using EShop.Core.Models;
using EShop.Core.Persistence;
using EShop.Core.Repositories;
using EShop.Tests.AsyncHelpers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EShop.Tests
{
    [TestFixture]
    public class ProductDAOMockTests : ProductDAOTestsBase
    {
        private Mock<EShopDbContext> dbContextMock;
        private Mock<DbSet<Product>> dbSetMock;

        [SetUp]
        public override void Setup()
        {
            dbContextMock = new Mock<EShopDbContext>(new DbContextOptions<EShopDbContext>());
            dbSetMock = new Mock<DbSet<Product>>();
            productDAO = new ProductDAO(dbContextMock.Object);
        }

        protected override void SetupProducts()
        {
            var products = GetTestProducts();
            // https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
            dbSetMock.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Product>(products.Provider));
            dbSetMock.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            dbSetMock.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            dbSetMock.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            // setup async enumeration
            dbSetMock.As<IAsyncEnumerable<Product>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));


            dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);
        }

        [Ignore("Requires mocking of extension methods Skip and Take - recommended is to use InMemory database.")]
        public override async Task GetAllProducts_WithPaging_ShouldReturnPagedProducts()
        {
            await Task.CompletedTask;
        }

        [Ignore("Requires mocking of extension method Count - recommended is to use InMemory database.")]
        public override async Task GetAllProductsCount_ShouldReturnCorrectCount()
        {
            await Task.CompletedTask;
        }

        protected override void SetupProductToGetById()
        {
            var products = GetTestProducts();
            dbSetMock.Setup(m => m.FindAsync(1)).ReturnsAsync(products.First(p => p.Id == 1));

            dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);
        }

        protected override Product SetupNewProductToAdd()
        {
            var newProduct = new Product { Id = 3, Name = "Product3", Price = 30.0m, Description = "Description3", ImgUri = "Uri3" };
            dbSetMock.Setup(m => m.Add(It.IsAny<Product>())).Callback<Product>(p => { });

            dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            return newProduct;
        }

        protected override void AssertNewProductAdded()
        {
            dbSetMock.Verify(m => m.Add(It.IsAny<Product>()), Times.Once);
            dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private Product existingProduct;
        private Product updatedProduct;

        protected override Product SetupExistingProductToUpdate()
        {
            existingProduct = new Product { Id = 1, Name = "Product1", Price = 10.0m, Description = "Description1", ImgUri = "Uri1" };
            dbSetMock.Setup(m => m.Find(1)).Returns(existingProduct);

            dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            updatedProduct = new Product { Id = 1, Name = "UpdatedProduct", Price = 15.0m, Description = "UpdatedDescription", ImgUri = "UpdatedUri" };

            return updatedProduct;
        }

        protected override void AssertExistingProductUpdated()
        {
            Assert.AreEqual(updatedProduct.Name, existingProduct.Name);
            Assert.AreEqual(updatedProduct.Price, existingProduct.Price);
            Assert.AreEqual(updatedProduct.ImgUri, existingProduct.ImgUri);
            Assert.AreEqual(updatedProduct.Description, existingProduct.Description);
            dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
