using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrationTool.Infrastructure;

public class MigrationApplier : IMigrationApplier
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationApplier(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ApplyMigrationResult> ApplyMigration(Type dbContextType)
    {
        var ctx = (DbContext)_serviceProvider
            .GetRequiredService(dbContextType);

        var pendingMigrations = await ctx.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
            return ApplyMigrationResult.UpToDate;

        await ctx.Database.MigrateAsync();
        return ApplyMigrationResult.Success;
    }
}
