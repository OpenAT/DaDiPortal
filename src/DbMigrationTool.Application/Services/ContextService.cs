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
    #region fields

    private readonly IConnectionStringProvider _connectionStringProvider;
    private readonly IMigrationsExplorer _migrationsExplorer;
    private readonly IMigrationApplier _migrationApplier;
    private readonly IDataSeeder _dataSeeder;

    #endregion

    #region ctors

    public ContextService(
        IMigrationsExplorer migrationsExplorer, 
        IConnectionStringProvider connectionStringProvider, 
        IMigrationApplier migrationApplier, 
        IDataSeeder dataSeeder)
    {
        _migrationsExplorer = migrationsExplorer;
        _connectionStringProvider = connectionStringProvider;
        _migrationApplier = migrationApplier;
        _dataSeeder = dataSeeder;
    }

    #endregion

    #region methods

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
        var migrationResult = await _migrationApplier.ApplyMigration(contextType);
        
        if (migrationResult == ApplyMigrationResult.Success)
        {
            if (contextType.Name == "ConfigurationDbContext")
            {
                if (!await _dataSeeder.SeedDataToConfigurationStore())
                    return ApplyMigrationResult.DataSeedToConfigStoreFailed;
            }
            else if (contextType.Name == "AspNetIdentityDbContext")
            {
                if (!await _dataSeeder.SeedDataToUserStore())
                    return ApplyMigrationResult.DataSeedToUserStoreFailed;
            }
        }

        return migrationResult;
    }

    #endregion
}
