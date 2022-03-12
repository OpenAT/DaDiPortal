using DaDiPortal.IdentityServer.Data;
using DbMigrationTool.Application.Infrastructure;
using DbMigrationTool.Application.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrationTool.Infrastructure;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddScoped<IDatabaseExplorer, DatabaseExplorer>()
            .AddScoped<IMigrationsExplorer, MigrationsExplorer>()
            .AddScoped<IMigrationApplier, MigrationApplier>()
            .AddSingleton<OperationalStoreOptions>()
            .AddSingleton<ConfigurationStoreOptions>()
            .AddDbContext<PersistedGrantDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddDbContext<ConfigurationDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddDbContext<AspNetIdentityDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient);

        return services;
    }

    private static void ConfigureDbContext(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        string identityServerAssemblyName = typeof(DaDiPortal.IdentityServer.Program)
            .Assembly
            .GetName()!
            .Name!;

        var connStrProvider = serviceProvider
            .GetRequiredService<IConnectionStringProvider>();

        dbContextOptionsBuilder.UseSqlServer(
            connStrProvider.GetConnectionString(), 
            b => b.MigrationsAssembly(identityServerAssemblyName));
    }
}