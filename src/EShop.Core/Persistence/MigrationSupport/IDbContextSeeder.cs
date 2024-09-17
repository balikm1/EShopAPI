using Microsoft.EntityFrameworkCore;

namespace EShop.Core.Persistence.MigrationSupport
{
    /// <summary>Represents interface for seeding <see cref="TDbContext"/>.</summary>
    /// <typeparam name="TDbContext">Type of DbContext.</typeparam>
    public interface IDbContextSeeder<in TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>Seeds <see cref="TDbContext"/>.</summary>
        /// <param name="context">Instance of <see cref="TDbContext"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task SeedAsync(TDbContext context, CancellationToken cancellationToken);
    }
}
