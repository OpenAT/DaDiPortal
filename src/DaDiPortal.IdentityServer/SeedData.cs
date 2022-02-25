using DaDiPortal.IdentityServer.Data;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DaDiPortal.IdentityServer;

public class SeedData
{
    public static async Task EnsureSeedData(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<AspNetIdentityDbContext>(o => o.UseSqlServer(connectionString));

        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AspNetIdentityDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddOperationalDbContext(opt => 
                opt.ConfigureDbContext = b => 
                    b.UseSqlServer(connectionString, b1 => b1.MigrationsAssembly(typeof(SeedData).Assembly.FullName)))
            .AddConfigurationDbContext(opt => 
                opt.ConfigureDbContext = b => 
                    b.UseSqlServer(connectionString, b1 => b1.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();

        EnsureSeedData(context);

        var ctx = scope.ServiceProvider.GetRequiredService<AspNetIdentityDbContext>();
        ctx.Database.Migrate();
        
        await EnsureUsers(scope);
    }

    private static async Task EnsureUsers(IServiceScope scope)
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var christian = await userMgr.FindByNameAsync("csp");

        if (christian == null)
        {
            christian = new IdentityUser
            {
                UserName = "csp",
                Email = "christian.spath@datadialog.net",
                EmailConfirmed = true
            };

            var result = await userMgr.CreateAsync(christian, "Pass123$");
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            result = await userMgr
                .AddClaimsAsync(christian, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Christian Spath"),
                    new Claim(JwtClaimTypes.GivenName, "Christian"),
                    new Claim(JwtClaimTypes.FamilyName, "Spath"),
                });

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
        }
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients.ToList())
                context.Clients.Add(client.ToEntity());

            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources.ToList())
                context.IdentityResources.Add(resource.ToEntity());

            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes.ToList())
                context.ApiScopes.Add(resource.ToEntity());

            context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in Config.ApiResources.ToList())
                context.ApiResources.Add(resource.ToEntity());

            context.SaveChanges();
        }
    }
}