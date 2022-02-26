using DaDiPortal.API.DataAccess;

namespace DaDiPortal.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication
            .CreateBuilder(args)
            .ConfigureServices();

        var app = builder
            .Build()
            .ConfigureWebApplication();

        app.Run();
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

        return builder;
    }

    private static WebApplication ConfigureWebApplication(this WebApplication webApp)
    {
        webApp.UseAuthentication();
        webApp.UseAuthorization();
        webApp.MapControllers();

        return webApp;
    }
}
