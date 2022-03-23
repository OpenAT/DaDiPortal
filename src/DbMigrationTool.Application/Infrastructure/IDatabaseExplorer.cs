namespace DbMigrationTool.Application.Infrastructure;

public interface IDatabaseExplorer
{
    Task<IEnumerable<string>> GetDatabases(string dbServer);
}
