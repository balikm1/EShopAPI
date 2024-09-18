using System;
using EShop.Core.Interfaces;
using EShop.Core.Persistence;
using EShop.Core.Persistence.MigrationSupport;
using EShop.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop.API.Extensions
{
    /// <summary>Represents class for registering dependencies to <see cref="IServiceCollection"/>.</summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registers local EShop services to <see cref="IServiceCollection"/>.</summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <returns>Modified instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEShopServices(this IServiceCollection services)
        {
            services.AddTransient<IProductDAO, ProductDAO>();

            return services;
        }


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
