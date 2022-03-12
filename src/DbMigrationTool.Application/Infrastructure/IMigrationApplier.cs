using DbMigrationTool.Application.Data;

namespace DbMigrationTool.Application.Infrastructure;

public interface IMigrationApplier
{
    Task<ApplyMigrationResult> ApplyMigration(Type dbContextType);
}
