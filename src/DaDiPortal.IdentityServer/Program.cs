using DaDiPortal.IdentityServer.Data;
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
        var assemblyName = typeof(Program)
            .Assembly
            .GetName()
            .Name;
        
        string connStr = builder
            .Configuration
            .GetConnectionString("DefaultConnection");

        builder.Services
            .AddDbContext<AspNetIdentityDbContext>(o => o.UseSqlServer(connStr, b => b.MigrationsAssembly(assemblyName)));

        builder.Services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AspNetIdentityDbContext>();

        builder.Services
            .AddIdentityServer()
            .AddAspNetIdentity<IdentityUser>()
            .AddConfigurationStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddOperationalStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddDeveloperSigningCredential();

        builder.Services
            .AddControllersWithViews();

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

        builder.Host.UseNLog();

        return builder;
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

