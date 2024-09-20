using EShop.Core.Persistence;
using EShop.Core.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EShop.Tests
{
    [TestFixture]
    public class ProductDAOInMemoryDatabaseTests : ProductDAODatabaseTestsBase
    {
        private SqliteConnection sqliteConnection;

        [SetUp]
        public override void Setup()
        {
            sqliteConnection = new SqliteConnection("Filename=:memory:");
            sqliteConnection.Open();

            var options = new DbContextOptionsBuilder<EShopDbContext>()
                .UseSqlite(sqliteConnection)
            .Options;

            dbContext = new EShopDbContext(options);
            dbContext.Database.EnsureCreated();

            productDAO = new ProductDAO(dbContext);
        }

        [TearDown]
        public override void Cleanup()
        {
            dbContext.Dispose();
            sqliteConnection.Close();
        }
    }
}
