using DbMigrationTool.Application.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrationTool.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddScoped<IDatabaseExplorer, DatabaseExplorer>();
    }
}
