using DaDiPortal.Portal.Pages;
using DaDiPortal.Portal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using NLog.Web;

namespace DaDiPortal.Portal;

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
            var builder = WebApplication
                .CreateBuilder(args)
                .ConfigureServices();

            var app = builder
                .Build()
                .ConfigureWebApp();

            _logger!.LogInformation("Configuration completed, running app");
            app.Run();
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
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services
            .AddHttpClient()
            .Configure<IdentityServerSettings>(builder.Configuration.GetSection("IdentityServerSettings"))
            .AddSingleton<ITokenService, TokenService>()
            .AddSingleton<ILogWrapper, LogWrapper>();

        builder.Services
           .AddAuthentication(o =>
           {
               o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
           })
           .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, o =>
           {
               o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               o.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
               o.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
               o.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
               o.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
               o.ResponseType = "code";
               o.SaveTokens = true;
               o.GetClaimsFromUserInfoEndpoint = true;
           });

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

        builder.Host.UseNLog();

        return builder;
    }

    private static WebApplication ConfigureWebApp(this WebApplication webApplication)
    {
        _logger = webApplication.Services
            .GetRequiredService<ILogger<WebApplication>>();

        if (!webApplication.Environment.IsDevelopment())
            webApplication
                .UseExceptionHandler("/Error")
                .UseHsts();

        webApplication
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization();

        webApplication.MapBlazorHub();
        webApplication.MapFallbackToPage("/_Host");

        return webApplication;
    }
}

