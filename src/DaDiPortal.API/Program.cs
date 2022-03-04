using DaDiPortal.API.DataAccess;
using NLog.Web;

namespace DaDiPortal.API;

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
                .ConfigureWebApplication();

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
        builder.Services
            .AddControllers();

        builder.Services
            .AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", opt =>
            {
                opt.Authority = "https://localhost:5443";
                opt.ApiName = "DaDiPortalApi";
            });

        builder.Services
            .AddDataAccessLayer(builder.Configuration);

        builder.Logging
            .ClearProviders()
            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

        builder.Host.UseNLog();
        
        return builder;
    }

    private static WebApplication ConfigureWebApplication(this WebApplication webApp)
    {
        _logger = webApp.Services
            .GetRequiredService<ILogger<WebApplication>>();
        
        webApp.UseAuthentication();
        webApp.UseAuthorization();
        webApp.MapControllers();

        return webApp;
    }
}

