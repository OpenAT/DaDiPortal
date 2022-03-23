using DaDiPortal.IdentityServer.Data;
using DbMigrationTool.Application.DTOs;
using DbMigrationTool.Application.Infrastructure;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DbMigrationTool.Infrastructure;

public class MigrationsExplorer : IMigrationsExplorer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public MigrationsExplorer(IServiceProvider serviceProvider, ILogger<MigrationsExplorer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task<IEnumerable<ContextDto>> GetContextsWithLatestMigrations()
    {
        _logger.LogInformation("Loading contexts with latest migrations");

        return Task.Run<IEnumerable<ContextDto>>(() =>
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
                    contextType,
                    GetLatestAvailableMigration(contextType)));

            return contextDtos;
        });
    }

    private string GetLatestAvailableMigration(Type contextType)
    {
        var identityServerAssembly = Assembly.GetAssembly(typeof(DaDiPortal.IdentityServer.Program))!;

        var migrationTypes = identityServerAssembly
            .GetTypes()
            .Where(x =>
                x.Namespace != null &&
                x.Namespace.Contains("Migrations"))
            .Where(type =>
            {
                var attributes = type
                    .GetCustomAttributes()
                    .ToArray();

                if (!attributes.Any(x => x is MigrationAttribute))
                    return false;

                var dbContextAttribute = attributes
                    .SingleOrDefault(x => x is DbContextAttribute) as DbContextAttribute;

                return
                    dbContextAttribute != null &&
                    dbContextAttribute.ContextType == contextType;
            })
            .ToArray();

        if (!migrationTypes.Any())
            return "NO MIGRATION FOUND";

        return migrationTypes
            .Select(x => x.GetCustomAttribute<MigrationAttribute>()!)
            .Select(x => x.Id)
            .OrderBy(x => x)
            .Last();
    }

    public async Task<string?> GetLatestAppliedMigration(Type contextType)
    {
        _logger.LogInformation($"Getting latest applied migration for context {contextType.Name}");

        var ctx = (DbContext)_serviceProvider
            .GetRequiredService(contextType);

        var migrations = await ctx
            .Database
            .GetAppliedMigrationsAsync();

        return migrations
            .OrderBy(x => x)
            .LastOrDefault();
    }
}
