using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.DTOs;
using DbMigrationTool.Application.Infrastructure;

namespace DbMigrationTool.Application.Services;

public interface IContextService
{
    void SetConnection(string server, string database);
    Task<IEnumerable<ContextDto>> GetContexts();
    Task<string?> GetLatestAppliedMigration(ContextDto context);
    Task<ApplyMigrationResult> ApplyMigration(Type contextType);
}

public class ContextService : IContextService
{
    private readonly IConnectionStringProvider _connectionStringProvider;
    private readonly IMigrationsExplorer _migrationsExplorer;
    private readonly IMigrationApplier _migrationApplier;

    public ContextService(IMigrationsExplorer migrationsExplorer, IConnectionStringProvider connectionStringProvider, IMigrationApplier migrationApplier)
    {
        _migrationsExplorer = migrationsExplorer;
        _connectionStringProvider = connectionStringProvider;
        _migrationApplier = migrationApplier;
    }

    public void SetConnection(string server, string database)
    {
        _connectionStringProvider.SetConnection(server, database);
    }

    public async Task<IEnumerable<ContextDto>> GetContexts()
    {
        return await _migrationsExplorer
            .GetContextsWithLatestMigrations();
    }

    public async Task<string?> GetLatestAppliedMigration(ContextDto context)
    {
        return await _migrationsExplorer.GetLatestAppliedMigration(context.ContextType);
    }

    public async Task<ApplyMigrationResult> ApplyMigration(Type contextType)
    {
        return await _migrationApplier.ApplyMigration(contextType);
    }
}
