using DaDiPortal.IdentityServer.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

namespace DaDiPortal.IdentityServer;

public static class Program
{
    private static ILogger? _logger;

    public static void Main(string[] args)
    {
        NLog.LogManager
            .Setup()
            .LoadConfigurationFromAppSettings();

        try
        {
            bool seed = args.Contains("/seed");
            if (seed)
                args = args
                    .Except(new[] { "/seed" })
                    .ToArray();

            var webAppBuilder = WebApplication
                .CreateBuilder(args)
                .ConfigureServices();

            if (seed)
                SeedData.EnsureSeedData(webAppBuilder.Configuration.GetConnectionString("DefaultConnection"));

            var webApp = webAppBuilder.Build();

            webApp
                .ConfigureWebApp()
                .Run();
        }
        catch (Exception exc)
        {
            _logger?.LogError(exc, "Unhandled exception");
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        //APPLY INITIAL MIGRATIONS WITH FOLLOWING COMMANDS
        //dotnet ef migrations add Operational_Initial --context PersistedGrantDbContext --output-dir Migrations/OperationalStore
        //dotnet ef migrations add Configuration_Initial --context ConfigurationDbContext --output-dir Migrations/ConfigurationStore
        //dotnet ef migrations add AspIdentity_Initial --context AspNetIdentityDbContext --output-dir Migrations/AspIdentityStore

        builder.Services
            .AddDbContext<AspNetIdentityDbContext>(o => o.ConfigureDbCtxOptions(builder.Configuration, DbConstants.HistoryTableAspNetIdentity, DbConstants.SchemaAspNetIdentity))
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AspNetIdentityDbContext>();

        builder.Services
            .AddIdentityServer()
            .AddAspNetIdentity<IdentityUser>()
            .AddConfigurationStore(opt =>
            {
                opt.DefaultSchema = "IdentityServer";
                opt.ConfigureDbContext = b => b.ConfigureDbCtxOptions(builder.Configuration, DbConstants.HistoryTableConfigurationStore, DbConstants.SchemaIdentityServer);
            })
            .AddOperationalStore(opt =>
            {
                opt.DefaultSchema = "IdentityServer";
                opt.ConfigureDbContext = b => b.ConfigureDbCtxOptions(builder.Configuration, DbConstants.HistoryTableOperationalStore, DbConstants.SchemaIdentityServer);
            })
            .AddDeveloperSigningCredential();

        builder.Services
            .AddControllersWithViews();

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

        builder.Host.UseNLog();

        return builder;
    }

    private static DbContextOptionsBuilder ConfigureDbCtxOptions(this DbContextOptionsBuilder optBuilder, IConfiguration config, string historyTableName, string historyTableSchema)
    {
        var assemblyName = typeof(Program)
                    .Assembly
                    .GetName()
                    .Name;
        
        string connStr = config
            .GetConnectionString("DefaultConnection");

        optBuilder.UseSqlServer(connStr, sqlOptBuilder => sqlOptBuilder
            .MigrationsAssembly(assemblyName)
            .MigrationsHistoryTable(historyTableName, historyTableSchema));

        return optBuilder;
    }

    private static WebApplication ConfigureWebApp(this WebApplication webApp)
    {
        _logger = webApp.Services
            .GetRequiredService<ILogger<WebApplication>>();

        if (!webApp.Environment.IsDevelopment())
            webApp
                .UseExceptionHandler("/Error")
                .UseHsts();

        webApp
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseIdentityServer()
            .UseAuthorization()
            .UseEndpoints(x => x.MapDefaultControllerRoute());

        return webApp;
    }
}

