using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.DTOs;
using DbMigrationTool.Application.Infrastructure;
using Microsoft.Extensions.Options;

namespace DbMigrationTool.Application.Services
{
    public interface IAppService
    {
        Task<IEnumerable<DatabaseServerDto>> GetDatabases();
    }

    internal class AppService : IAppService
    {
        private readonly IDatabaseExplorer _dbExplorer;
        private readonly DatabaseServersSettings _dbServerSettings;

        public AppService(IDatabaseExplorer dbExplorer, IOptions<DatabaseServersSettings> dbServerSettings)
        {
            _dbExplorer = dbExplorer;
            _dbServerSettings = dbServerSettings.Value;
        }

        public async Task<IEnumerable<DatabaseServerDto>> GetDatabases()
        {
            var allDatabases = new List<DatabaseOfServer>();

            foreach (string dbServer in _dbServerSettings.DatabaseServers)
            {
                var databases = await _dbExplorer.GetDatabases(dbServer);
                
                var databasesOnServer = databases
                    .Select(x => new DatabaseOfServer(dbServer, x));

                allDatabases.AddRange(databasesOnServer);
            }

            return allDatabases
                .GroupBy(x => x.Server)
                .Select(x => new DatabaseServerDto(x.Key, x.Select(y => new DatabaseDto(y.Database)).ToArray()))
                .ToArray();
        }
    }
}
