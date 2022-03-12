using DaDiPortal.IdentityServer.Data;
using DbMigrationTool.Application.DTOs;
using DbMigrationTool.Application.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DbMigrationTool.Infrastructure;

public class MigrationsExplorer : IMigrationsExplorer
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationsExplorer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<ContextDto>> GetContextsWithLatestMigrations()
    {
        var contextTypes = new Type[]
        {
            typeof(PersistedGrantDbContext),
            typeof(ConfigurationDbContext),
            typeof(AspNetIdentityDbContext)
        };

        var contextDtos = new List<ContextDto>();

        foreach (var contextType in contextTypes)
            contextDtos.Add(new ContextDto(
                contextType.Name,
                GetLatestAvailableMigration(contextType),
                await GetLatestAppliedMigration(contextType)));

        return contextDtos;
    }

    private string GetLatestAvailableMigration(Type contextType)
    {
        return ((DbContext)_serviceProvider
            .GetRequiredService(contextType))
            .Database
            .GetMigrations()
            .Last();
    }

    private async Task<string?> GetLatestAppliedMigration(Type contextType)
    {
        var ctx = (DbContext)_serviceProvider
            .GetRequiredService(contextType);

        string connStr = ctx.Database.GetConnectionString()!;

        var migrations = await ctx
            .Database
            .GetAppliedMigrationsAsync();

        return migrations
            .OrderBy(x => x)
            .LastOrDefault();
    }
}
