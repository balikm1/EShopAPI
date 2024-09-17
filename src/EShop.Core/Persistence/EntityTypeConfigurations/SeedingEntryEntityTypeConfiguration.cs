using EShop.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Core.Persistence.EntityTypeConfigurations
{
    /// <summary>Represents implementation of <see cref="IEntityTypeConfiguration{TEntity}"/> for <see cref="SeedingEntry"/>.</summary>
    class SeedingEntryEntityTypeConfiguration : IEntityTypeConfiguration<SeedingEntry>
    {
        /// <summary>Configures the entity of <see cref="SeedingEntry"/>.</summary>
        /// <param name="builder"> The builder to be used to configure the entity type. </param>
        public void Configure(EntityTypeBuilder<SeedingEntry> builder)
        {
            builder.ToTable("__SeedingHistory");
            builder.HasKey(s => s.Name);
        }
    }
}
