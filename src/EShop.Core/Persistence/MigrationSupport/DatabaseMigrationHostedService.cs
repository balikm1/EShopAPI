using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace EShop.Core.Persistence.MigrationSupport
{
    /// <summary>Represents implementation of <see cref="IHostedService"/> for database migrations.</summary>
    public class DatabaseMigrationHostedService<TDbContext> : IHostedService
        where TDbContext : DbContext
    {
        private readonly ILogger<DatabaseMigrationHostedService<TDbContext>> logger;
        private readonly IServiceProvider serviceProvider;

        /// <summary>Initializes new instance of <see cref="DatabaseMigrationHostedService{TDbContext}"/>.</summary>
        /// <param name="logger">Instance of <see cref="ILogger{DatabaseMigrationHostedService}"/>.</param>
        /// <param name="serviceProvider">Instance of <see cref="IServiceProvider"/>.</param>
        public DatabaseMigrationHostedService(
            ILogger<DatabaseMigrationHostedService<TDbContext>> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TDbContext).Name}");

                using var scope = serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

                var retry = Policy.Handle<Exception>()
                    .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(8),
                    });

                await retry.ExecuteAsync(async () =>
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        await context.Database.MigrateAsync(cancellationToken);
                    }
                });

                logger.LogInformation($"Migrated database associated with context {typeof(TDbContext).Name}");

                var seeder = scope.ServiceProvider.GetService<IDbContextSeeder<TDbContext>>();
                if (seeder == null)
                {
                    logger.LogInformation($"Database seeder is not registered for {typeof(TDbContext).Name}, skipping seeding.");
                    return;
                }

                logger.LogInformation($"Seeding is going to be applied with context {typeof(TDbContext).Name}");

                await retry.ExecuteAsync(async () =>
                {
                    await seeder.SeedAsync(context, cancellationToken);
                });

                logger.LogInformation($"Seeding is completed with context {typeof(TDbContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TDbContext).Name}");
                throw;
            }
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
