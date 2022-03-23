using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrationTool.Application;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration config)
    {
        return services
            .Configure<DatabaseServersSettings>(config.GetSection("DatabaseServersSettings"))
            .AddScoped<IAppService, AppService>()
            .AddScoped<IContextService, ContextService>()
            .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
    }
}
