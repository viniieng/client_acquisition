using ClientAcquisition.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientAcquisition.Api;

public static class DatabaseInitializer
{
    public static void Initialize(ApplicationDbContext context, ILogger logger)
    {
        var applied = context.Database.GetAppliedMigrations().ToList();
        var pending = context.Database.GetPendingMigrations().ToList();

        // The database may have been provisioned manually (e.g. tables created directly in the
        // Supabase SQL editor). In that case the schema already exists but EF has no migration
        // history, so a plain Migrate() would fail trying to CREATE TABLE objects that exist.
        // When that happens, baseline the history so EF treats the schema as already applied.
        if (applied.Count == 0 && pending.Count > 0 && SchemaAlreadyExists(context))
        {
            BaselineMigrations(context, pending);
            logger.LogInformation(
                "Existing database schema detected with no migration history. Baselined migrations: {Migrations}.",
                string.Join(", ", pending));
        }

        context.Database.Migrate();
    }

    private static bool SchemaAlreadyExists(ApplicationDbContext context)
    {
        var connection = context.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose)
        {
            connection.Open();
        }

        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT (to_regclass('public.""Customers""') IS NOT NULL)
                   AND (to_regclass('public.""Orders""') IS NOT NULL)
                   AND (to_regclass('public.""OrderItems""') IS NOT NULL);";

            return command.ExecuteScalar() is true;
        }
        finally
        {
            if (shouldClose)
            {
                connection.Close();
            }
        }
    }

    private static void BaselineMigrations(ApplicationDbContext context, IEnumerable<string> migrationIds)
    {
        var historyRepository = context.GetService<IHistoryRepository>();
        context.Database.ExecuteSqlRaw(historyRepository.GetCreateIfNotExistsScript());

        var productVersion = ProductInfo.GetVersion();
        foreach (var migrationId in migrationIds)
        {
            var insertScript = historyRepository.GetInsertScript(new HistoryRow(migrationId, productVersion));
            context.Database.ExecuteSqlRaw(insertScript);
        }
    }
}
