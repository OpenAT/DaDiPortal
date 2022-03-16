using DaDiPortal.IdentityServer;
using DaDiPortal.IdentityServer.Data;
using DbMigrationTool.Application.Infrastructure;
using DbMigrationTool.Application.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
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
            .AddScoped<IDataSeeder, DataSeeder>()
            .AddSingleton<OperationalStoreOptions>(new OperationalStoreOptions() { DefaultSchema = DbConstants.SchemaIdentityServer })
            .AddSingleton<ConfigurationStoreOptions>(new ConfigurationStoreOptions() { DefaultSchema = DbConstants.SchemaIdentityServer })
            .AddDbContext<PersistedGrantDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddDbContext<ConfigurationDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient)
            .AddDbContext<AspNetIdentityDbContext>(ConfigureDbContext, ServiceLifetime.Transient, ServiceLifetime.Transient);

        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AspNetIdentityDbContext>();

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

        string historyTableName;
        string historyTableSchema;

        if (dbContextOptionsBuilder.Options.ContextType == typeof(PersistedGrantDbContext))
        {
            historyTableName = DbConstants.HistoryTableOperationalStore;
            historyTableSchema = DbConstants.SchemaIdentityServer;
        }
        else if (dbContextOptionsBuilder.Options.ContextType == typeof(ConfigurationDbContext))
        {
            historyTableName = DbConstants.HistoryTableConfigurationStore;
            historyTableSchema = DbConstants.SchemaIdentityServer;
        }
        else if (dbContextOptionsBuilder.Options.ContextType == typeof(AspNetIdentityDbContext))
        {
            historyTableName = DbConstants.HistoryTableAspNetIdentity;
            historyTableSchema = DbConstants.SchemaAspNetIdentity;
        }
        else
            throw new Exception();

        dbContextOptionsBuilder.UseSqlServer(
            connStrProvider.GetConnectionString(),
            b => b.MigrationsAssembly(identityServerAssemblyName).MigrationsHistoryTable(historyTableName, historyTableSchema));
    }
}