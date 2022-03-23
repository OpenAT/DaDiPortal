using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.DTOs;
using DbMigrationTool.Application.Infrastructure;
using Microsoft.Extensions.Options;

namespace DbMigrationTool.Application.Services
{
    public interface IAppService
    {
        Task<IEnumerable<string>> GetServers();
        Task<IEnumerable<string>> GetDatabases(string server);
    }

    internal class AppService : IAppService
    {
        private readonly IDatabaseExplorer _dbExplorer;
        private readonly DatabaseServersSettings _dbServerSettings;

        public AppService(
            IDatabaseExplorer dbExplorer, 
            IOptions<DatabaseServersSettings> dbServerSettings)
        {
            _dbExplorer = dbExplorer;
            _dbServerSettings = dbServerSettings.Value;
        }

        public Task<IEnumerable<string>> GetServers()
        {
            return Task.Run(() => _dbServerSettings.DatabaseServers);
        }

        public async Task<IEnumerable<string>> GetDatabases(string server)
        {
            var databases = await _dbExplorer
                .GetDatabases(server);

            return databases
                .ToArray();
        }
    }
}
