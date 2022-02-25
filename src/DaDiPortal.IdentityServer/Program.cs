using Microsoft.EntityFrameworkCore;

namespace DaDiPortal.IdentityServer;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication
            .CreateBuilder(args)
            .ConfigureServices();

        var app = builder
            .Build()
            .ConfigureWebApp();

        app.Run();
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
            .AddIdentityServer()
            .AddConfigurationStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddOperationalStore(opt => opt.ConfigureDbContext = b => b.UseSqlServer(connStr, b1 => b1.MigrationsAssembly(assemblyName)))
            .AddDeveloperSigningCredential();

        return builder;
    }

    private static WebApplication ConfigureWebApp(this WebApplication webApp)
    {
        webApp
            .UseIdentityServer();

        return webApp;
    }
}

