using DaDiPortal.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DaDiPortal.IdentityServer;

public static class Program
{
    public static void Main(string[] args)
    {
        bool seed = args.Contains("/seed");
        if (seed)
            args = args
                .Except(new[] { "/seed" })
                .ToArray();

        var builder = WebApplication
            .CreateBuilder(args)
            .ConfigureServices();

        if (seed)
            SeedData.EnsureSeedData(builder.Configuration.GetConnectionString("DefaultConnection"));

        var app = builder.Build();
        app.ConfigureWebApp();

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

        return builder;
    }

    private static WebApplication ConfigureWebApp(this WebApplication webApp)
    {
        webApp.UseStaticFiles()
            .UseRouting()
            .UseIdentityServer()
            .UseAuthorization()
            .UseEndpoints(x => x.MapDefaultControllerRoute());

        return webApp;
    }
}

