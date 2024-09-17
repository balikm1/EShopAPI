using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Core.Persistence
{
    /// <summary>Entity framework DB context.</summary>
    public class EShopDbContext : DbContext
    {
        /// <summary>Initializes new instance of <see cref="EShopDbContext"/>.</summary>
        /// <param name="options">Instance of <see cref="DbContextOptions{EShopDbContext}"/>.</param>
        public EShopDbContext(DbContextOptions<EShopDbContext> options)
            : base(options)
        { }

        /// <summary>Gets or sets <see cref="DbSet{TEntity}"/> of seeding SQL scripts executed during seeding.</summary>
        internal virtual DbSet<SeedingEntry> SeedingEntries { get; set; } = null!;

        public virtual DbSet<Product> Products { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EShopDbContext).Assembly);
        }
    }
}
