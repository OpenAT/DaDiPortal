using IdentityServer4.Models;

namespace DaDiPortal.IdentityServer;

public static class Config
{
    private const string DADI_PORTAL_API = "DaDiPortalApi";

    public static IEnumerable<IdentityResource> IdentityResources => new[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource
        {
            Name = "role",
            UserClaims = { "role" }
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new[]
    {
        new ApiScope($"{DADI_PORTAL_API}.read"),
        new ApiScope($"{DADI_PORTAL_API}.write"),
    };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("DaDiPortalApi")
        {
            Scopes = 
            { 
                $"{DADI_PORTAL_API}.read", 
                $"{DADI_PORTAL_API}.write"
            },
            ApiSecrets = { new Secret("n&((p-{CM6=u9JwT".Sha256()) },
            UserClaims = { "role" }
        }
    };

    public static IEnumerable<Client> Clients => new[]
    {
        new Client //m2m client credentials flow client
        {
            ClientId = "m2m.client",
            ClientName = "Client Credentials Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = {new Secret(",kKR4&2ssJ5p^G'~".Sha256())},
            AllowedScopes =
            {
                $"{DADI_PORTAL_API}.read",
                $"{DADI_PORTAL_API}.write"
            },
        },        
        new Client //interactive client using code flow + pkce
        {
            ClientId = "interactive",
            ClientSecrets = {new Secret("3!t#+5h;J{Z;n~c~".Sha256())},

            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = {"https://localhost:5444/signin-oidc"},
            FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
            PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

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
        },
    };
}
