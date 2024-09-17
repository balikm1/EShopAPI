using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EShop.Core.Persistence
{
    /// <summary>Represents implementation of <see cref="IDesignTimeDbContextFactory{TContext}"/> for designing database context.</summary>
    public class EShopDesignTimeDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
    {
        /// <summary>Creates a new instance of a derived context.</summary>
        /// <param name="args"> Arguments provided by the design-time service. </param>
        /// <returns> An instance of <typeparamref name="EShopDbContext" />. </returns>
        public EShopDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=127.0.0.1;Database=EShopDB;User Id=sa;Password=Strong@Passw0rd",
                opt => { opt.MigrationsHistoryTable("MigrationHistory"); });

            return new EShopDbContext(optionsBuilder.Options);
        }
    }
}
