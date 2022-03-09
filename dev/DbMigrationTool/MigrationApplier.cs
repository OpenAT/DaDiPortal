using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DbMigrationTool;

internal class MigrationApplier : IHostedService
{
    private readonly PersistedGrantDbContext _persistedGrantDbCtx;
    private readonly Terminal _terminal;

    public MigrationApplier(PersistedGrantDbContext persistedGrantDbCtx, Terminal terminal)
    {
        _persistedGrantDbCtx = persistedGrantDbCtx;
        _terminal = terminal;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var pendingMigrations = (await _persistedGrantDbCtx
            .Database
            .GetPendingMigrationsAsync()).ToArray();

        if (pendingMigrations.Any())
        {
            _terminal.Info("Pending Migrations for PersistedGrantDbContext");

            int index = 1;
            foreach (var pendingMigration in pendingMigrations)
                _terminal.Info($"\t{index++}: {pendingMigration}");

            _terminal.GetInt("\nSelect migration", out int migrationIndex);

            string migrationName = pendingMigrations[migrationIndex - 1];
            _terminal.GetYesNo($"Do you really want to apply migration '{migrationName}'", out bool answer);

            if (answer)
            {
                try
                {
                    await _persistedGrantDbCtx
                        .GetInfrastructure()
                        .GetRequiredService<IMigrator>()
                        .MigrateAsync(migrationName);

                    _terminal.Success($"Migration '{migrationName}' successfully applied");
                }
                catch (Exception exc)
                {
                    _terminal.Error($"Unable to apply migration '{migrationName}'", exc);
                }
            }
            else
                _terminal.Info("Aborted. Migration will not be performed.");
        }
        else
            _terminal.Info("No pending migrations for PersistedGrantDbContext");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _persistedGrantDbCtx.DisposeAsync();
    }
}
