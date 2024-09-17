using EShop.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Core.Persistence.EntityTypeConfigurations
{
    /// <summary>Represents implementation of <see cref="IEntityTypeConfiguration{TEntity}"/> for <see cref="Product"/>.</summary>
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        /// <summary>Configures the entity of <see cref="Product"/>.</summary>
        /// <param name="builder"> The builder to be used to configure the entity type. </param>
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "dbo");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsUnicode(true);
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Name).IsRequired();

            builder.Property(p => p.ImgUri).HasMaxLength(2000);
            builder.Property(p => p.ImgUri).IsRequired();

            builder.Property(p => p.Price).IsRequired();

            builder.Property(p => p.Description).HasColumnType("nvarchar(max)");
        }
    }
}
