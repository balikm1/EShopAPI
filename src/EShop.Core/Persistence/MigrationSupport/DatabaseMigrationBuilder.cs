using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Core.Persistence.MigrationSupport
{
    /// <summary>Represents class for building database migration.</summary>
    /// <typeparam name="TDbContext">Type of DbContext.</typeparam>
    public class DatabaseMigrationBuilder<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>Initializes new instance of <see cref="DatabaseMigrationBuilder{TDbContext}"/>.</summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        public DatabaseMigrationBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>Gets instance of <see cref="IServiceCollection"/>.</summary>
        public IServiceCollection Services { get; private set; }

        /// <summary>Registers instance of <see cref="IDbContextSeeder{TDbContext}"/> for seeding DbContext.</summary>
        /// <typeparam name="TDbContextSeeder">Type of DbContextSeeder.</typeparam>
        public void AddSeeder<TDbContextSeeder>()
            where TDbContextSeeder : class, IDbContextSeeder<TDbContext>
        {
            Services.AddTransient<IDbContextSeeder<TDbContext>, TDbContextSeeder>();
        }
    }
}
