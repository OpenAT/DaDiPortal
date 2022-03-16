using DaDiPortal.IdentityServer.Data;
using DbMigrationTool.Application.Services;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DbMigrationTool.Infrastructure;

public class DataSeeder : IDataSeeder
{
    #region constants

    private const string DADI_PORTAL_API = "DaDiPortalApi";

    #endregion

    #region fields

    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    private ConfigurationDbContext _configDbCtx;
    
    #endregion

    #region ctors

    public DataSeeder(ILogger<DataSeeder> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region methods

    public async Task<bool> SeedDataToConfigurationStore()
    {
        _logger.LogInformation("Seeding data for configuration store");
        
        _configDbCtx = _serviceProvider
            .GetRequiredService<ConfigurationDbContext>();

        if (!ConfigurationStoreIsEmpty())
        {
            _logger.LogError("Data seeding in configuration store is not allowed because it is not empty");
            return false;
        }

        AddClients();
        AddIdentityResources();
        AddApiScopes();
        AddApiResources();

        try
        {
            await _configDbCtx.SaveChangesAsync();
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "Unable to persist seeded data");
            return false;
        }

        _logger.LogInformation("Data successfully seeded");
        return true;
    }

    public async Task<bool> SeedDataToUserStore()
    {
        _logger.LogInformation("Seeding data for user store");

        var userManager = _serviceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        var user = new IdentityUser
        {
            UserName = "csp",
            Email = "christian.spath@datadialog.net",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Pass123$");
        if (!result.Succeeded)
        {
            _logger.LogError("Unable to create user.");
            return false;
        }

        result = await userManager
            .AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Christian Spath"),
                new Claim(JwtClaimTypes.GivenName, "Christian"),
                new Claim(JwtClaimTypes.FamilyName, "Spath"),
            });

        if (!result.Succeeded)
        {
            _logger.LogError("Unable to seed user data");
            return false;
        }

        _logger.LogInformation("Data successfully seeded");
        return true;
    }

    private bool ConfigurationStoreIsEmpty()
    {
        return
            !_configDbCtx.Clients.Any() &&
            !_configDbCtx.IdentityResources.Any() &&
            !_configDbCtx.ApiScopes.Any() &&
            !_configDbCtx.ApiResources.Any();
    }

    private void AddClients()
    {
        var m2mClient = new Client //m2m client credentials flow client
        {
            ClientId = "m2m.client",
            ClientName = "Client Credentials Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret(",kKR4&2ssJ5p^G'~".Sha256()) },
            AllowedScopes =
            {
                $"{DADI_PORTAL_API}.read",
                $"{DADI_PORTAL_API}.write"
            },
        };

        var interactiveClient = new Client //interactive client using code flow + pkce
        {
            ClientId = "interactive",
            ClientSecrets = { new Secret("3!t#+5h;J{Z;n~c~".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = { "https://localhost:5444/signin-oidc" },
            FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
            PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },

            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
                $"{DADI_PORTAL_API}.read",
                $"{DADI_PORTAL_API}.write"
            },
            RequirePkce = true,
            RequireConsent = true,
            AllowPlainTextPkce = false
        };

        _configDbCtx
            .Clients
            .AddRange(new[] { m2mClient.ToEntity(), interactiveClient.ToEntity() });
    }

    private void AddIdentityResources()
    {
        _configDbCtx.IdentityResources.Add(new IdentityResources.OpenId().ToEntity());
        _configDbCtx.IdentityResources.Add(new IdentityResources.Profile().ToEntity());

        var roleResource = new IdentityResource
        {
            Name = "role",
            UserClaims = { "role" }
        };

        _configDbCtx.IdentityResources.Add(roleResource.ToEntity());
    }

    private void AddApiScopes()
    {
        var readScope = new ApiScope($"{DADI_PORTAL_API}.read");
        var writeScope = new ApiScope($"{DADI_PORTAL_API}.write");

        _configDbCtx.ApiScopes.Add(readScope.ToEntity());
        _configDbCtx.ApiScopes.Add(writeScope.ToEntity());
    }

    private void AddApiResources()
    {
        var resource = new ApiResource("DaDiPortalApi")
        {
            Scopes =
            {
                $"{DADI_PORTAL_API}.read",
                $"{DADI_PORTAL_API}.write"
            },
            ApiSecrets = { new Secret("n&((p-{CM6=u9JwT".Sha256()) },
            UserClaims = { "role" }
        };

        _configDbCtx.ApiResources.Add(resource.ToEntity());
    }

    #endregion
}
