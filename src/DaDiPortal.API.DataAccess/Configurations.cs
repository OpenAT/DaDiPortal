using DaDiPortal.API.DataAccess.DataServices;
using DaDiPortal.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaDiPortal.API.DataAccess;

public static class Configurations
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddDbContext<ApiCtx>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")))
            .AddScoped<IDonationsDataService, DonationsDataService>();

        return services;
    }
}
