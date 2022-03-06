using DaDiPortal.IdentityServer.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DbMigrationTool;

public static class Program
{
    public static void Main(string[] args)
    {
        var assemblyName = typeof(Program)
            .Assembly
            .GetName()
            .Name;

        var terminal = new Terminal()
            .Info("Enter infos or press ctrl+c to quit\n")
            .GetString("Database server", out string dbServer)
            .GetString("Database name", out string dbName);

        string connStr = GetConnStr(dbServer, dbName);

        var services = new ServiceCollection()
            .AddDbContext<AspNetIdentityDbContext>(o => o.UseSqlServer(connStr, b => b.MigrationsAssembly(assemblyName)))
            .AddIdentityServer()
            .AddConfigurationStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddOperationalStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)));
        
        terminal.Info($"Applying migrations to:\n{connStr}");
        terminal.Info("Press key to quit");
        Console.ReadKey();
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