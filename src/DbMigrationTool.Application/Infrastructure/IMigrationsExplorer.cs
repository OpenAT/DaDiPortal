using DbMigrationTool.Application.DTOs;

namespace DbMigrationTool.Application.Infrastructure;

public interface IMigrationsExplorer
{
    Task<IEnumerable<ContextDto>> GetContextsWithLatestMigrations();
    Task<string?> GetLatestAppliedMigration(Type contextType);
}
