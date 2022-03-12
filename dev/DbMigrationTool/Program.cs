/*
 * Initial Migrations created with
 *  Add-Migration InitialIdentityServerMigration -c PersistedGrantDbContext -OutputDir "Migrations/GrantDbContext"
 *  Add-Migration InitialIdentityServerConfigMigration -c ConfigurationDbContext -OutputDir "Migrations/ConfigDbContext"
 */

using DaDiPortal.IdentityServer.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DbMigrationTool;

public static class Program
{
    private static bool _inApplyMode;
    private static string _connStr;

    public static async Task Main(string[] args)
    {
        _inApplyMode = args.Contains("ApplyMigrations");

        var host = new HostBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        await host.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        string assemblyName = typeof(DaDiPortal.IdentityServer.Program)
            .Assembly
            .GetName()!
            .Name!;

        if (!_inApplyMode)
            _connStr = "Server=.;Database=TestDb;Integrated Security=true;MultipleActiveResultSets=true;";
        else
        {
            var terminal = new Terminal()
                .Info("Enter infos or press ctrl+c to quit\n")
                .GetString("Database server", out string dbServer)
                .GetString("Database name", out string dbName);

            services
                .AddHostedService<MigrationApplier>()
                .AddSingleton(terminal);

            _connStr = GetConnStr(dbServer, dbName);

            terminal.Info($"\nUsing connection string: {_connStr}\n");
        }

        services
            .AddDbContext<AspNetIdentityDbContext>(o => o.UseSqlServer(_connStr, b => b.MigrationsAssembly(assemblyName)))
            .AddIdentityServer()
            .AddConfigurationStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(_connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddOperationalStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(_connStr, b1 => b1.MigrationsAssembly(assemblyName)));
    }

    private static string GetConnStr(string dbServer, string dbName)
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = dbServer,
            InitialCatalog = dbName,
            IntegratedSecurity = true
        }.ToString();
    }
}