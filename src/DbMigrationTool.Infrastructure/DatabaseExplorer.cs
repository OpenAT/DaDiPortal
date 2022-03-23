using DbMigrationTool.Application.Infrastructure;
using Extensions;
using System.Text;

namespace DbMigrationTool.Infrastructure;

public class DatabaseExplorer : SqlDataServiceBase, IDatabaseExplorer
{
    public async Task<IEnumerable<string>> GetDatabases(string dbServer)
    {
        _connStr = $"Server={dbServer};Database=master;integrated security=true;MultipleActiveResultSets=true;";
        var rows = await GetRows("select name from sys.databases where database_id > 4");

        return rows
            .Select(x => x.GetString("name"))
            .OrderBy(x => x)
            .ToArray();
    }
}
