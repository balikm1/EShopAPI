using System;
using EShop.Core.Persistence;
using EShop.Core.Persistence.MigrationSupport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registers SQL Server database context to <see cref="IServiceCollection"/>.</summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/>.</param>
        /// <returns>Modified instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            string connectionString = configuration.GetSection("Database").Get<DbContextOptions>().ConnectionString;

            // Registering DbContext.
            services
                .AddEntityFrameworkSqlServer()
                .AddPooledDbContextFactory<EShopDbContext>
                ((serviceProvider, optionsBuilder) =>
                {
                    using var scope = serviceProvider.CreateScope();
                    optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
                    {
                        sqlServerOptionsBuilder.MigrationsHistoryTable("__MigrationHistory");
                        sqlServerOptionsBuilder.MigrationsAssembly("EShop.Core");
                        sqlServerOptionsBuilder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                    });
                    optionsBuilder.UseInternalServiceProvider(serviceProvider);
                });

            services.AddScoped(p => p.GetRequiredService<IDbContextFactory<EShopDbContext>>().CreateDbContext());

            // ONLY FOR DEVELOPMENT ENVIRONMENT! : Registering migration related services for DbContext.
            if (hostEnvironment.IsDevelopment())
            {
                services.AddDatabaseMigrationHostedService<EShopDbContext>()
                    .AddSeeder<EShopDbContextSeeder>();
            }

            return services;
        }
    }
}
