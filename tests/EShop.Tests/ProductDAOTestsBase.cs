using EShop.Core.Models;
using EShop.Core.Repositories;

namespace EShop.Tests
{
    public abstract class ProductDAOTestsBase
    {
        protected ProductDAO productDAO;

        protected int existingProductCount = 0;

        [SetUp]
        public abstract void Setup();

        [TearDown]
        public virtual void Cleanup() { }

        protected virtual IQueryable<Product> GetTestProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0m, Description = "Description1", ImgUri = "Uri1" },
                new Product { Id = 2, Name = "Product2", Price = 20.0m, Description = "Description2", ImgUri = "Uri2" }
            }.AsQueryable();
        }

        protected abstract void SetupProducts();

        // Common test logic marked as virtual and Test
        [Test]
        public virtual async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            SetupProducts();

            // Act
            var result = await productDAO.GetAllProducts();

            // Assert
            Assert.AreEqual(existingProductCount  + 2, result.Count(), "The number of products returned should be 2.");
        }

        [Test]
        public virtual async Task GetAllProducts_WithPaging_ShouldReturnPagedProducts()
        {
            // Arrange
            SetupProducts();

            // Act
            var result = await productDAO.GetAllProducts(1, 1);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public virtual async Task GetAllProductsCount_ShouldReturnCorrectCount()
        {
            //Arrange
            SetupProducts();

            // Act
            var count = await productDAO.GetAllProductsCount();

            // Assert
            Assert.AreEqual(existingProductCount + 2, count);
        }

        protected abstract void SetupProductToGetById();

        [Test]
        public virtual async Task GetProductById_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            SetupProductToGetById();

            // Act
            var product = await productDAO.GetProductById(1);

            // Assert
            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.Id);
        }

        protected abstract Product SetupNewProductToAdd();
        protected abstract void AssertNewProductAdded();

        [Test]
        public virtual async Task SaveProduct_ShouldAddNewProduct()
        {
            // Arrange
            var newProduct = SetupNewProductToAdd();

            // Act
            await productDAO.SaveProduct(newProduct);

            // Assert
            AssertNewProductAdded();
        }

        protected abstract Product SetupExistingProductToUpdate();
        protected abstract void AssertExistingProductUpdated();

        [Test]
        public virtual async Task UpdateProduct_ShouldUpdateExistingProduct()
        {
            // Arrange
            var updatedProduct = SetupExistingProductToUpdate();

            // Act
            await productDAO.UpdateProduct(updatedProduct);

            // Assert
            AssertExistingProductUpdated();
        }
    }
}
