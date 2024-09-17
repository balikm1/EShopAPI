using EShop.Core.Models;
using EShop.Core.Persistence.MigrationSupport;
using Microsoft.EntityFrameworkCore;

namespace EShop.Core.Persistence
{
    /// <summary>Represents implementation of <see cref="IDbContextSeeder{JobPublisherDbContext}"/>.</summary>
    public class EShopDbContextSeeder : IDbContextSeeder<EShopDbContext>
    {
        /// <inheritdoc />
        public async Task SeedAsync(EShopDbContext context, CancellationToken cancellationToken)
        {
            var assembly = typeof(EShopDbContextSeeder).Assembly;
            var files = assembly.GetManifestResourceNames();

            var executedSeedings = context.SeedingEntries.ToArray();
            var filePrefix = $"{assembly.GetName().Name}.Persistence.Seedings.";
            foreach (var file in files.Where(f => f.StartsWith(filePrefix) && f.EndsWith(".sql"))
                                      .Select(f => new
                                      {
                                          PhysicalFile = f,
                                          LogicalFile = f.Replace(filePrefix, string.Empty)
                                      })
                                      .OrderBy(f => f.LogicalFile))
            {
                if (executedSeedings.Any(e => e.Name == file.LogicalFile))
                    continue;

                string command = string.Empty;
                using (Stream? stream = assembly.GetManifestResourceStream(file.PhysicalFile))
                {
                    if (stream == null)
                        continue;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        command = reader.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                var strategy = context.Database.CreateExecutionStrategy();

                await strategy.Execute(async () =>
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Database.ExecuteSqlRaw(command.Replace("{", "{{").Replace("}", "}}"));
                            context.SeedingEntries.Add(new SeedingEntry() { Name = file.LogicalFile });
                            await context.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                });
            }
        }
    }
}
