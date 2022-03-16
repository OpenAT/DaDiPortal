using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbMigrationTool.Infrastructure;

public class MigrationApplier : IMigrationApplier
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    public MigrationApplier(ILogger<MigrationApplier> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<ApplyMigrationResult> ApplyMigration(Type dbContextType)
    {
        _logger.LogInformation($"Applying migration for context {dbContextType.Name}");

        var ctx = (DbContext)_serviceProvider
            .GetRequiredService(dbContextType);

        var pendingMigrations = await ctx.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
        {
            _logger.LogInformation("No pending migrations found");
            return ApplyMigrationResult.UpToDate;
        }

        try
        {
            await ctx.Database.MigrateAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "Migration failed");
            return ApplyMigrationResult.MigrationError;
        }

        return ApplyMigrationResult.Success;
    }
}
