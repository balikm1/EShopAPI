using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Core.Persistence.MigrationSupport
{
    /// <summary>Represents extension class for <see cref="IServiceCollection"/>.</summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registers database migration hosted service for migrating database.</summary>
        /// <typeparam name="TDbContext">Type of DbContext.</typeparam>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <returns></returns>
        public static DatabaseMigrationBuilder<TDbContext> AddDatabaseMigrationHostedService<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddHostedService<DatabaseMigrationHostedService<TDbContext>>();
            return new DatabaseMigrationBuilder<TDbContext>(services);
        }
    }
}
