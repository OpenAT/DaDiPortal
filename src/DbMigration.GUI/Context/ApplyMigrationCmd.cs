using DbMigrationTool.Application.Data;
using DbMigrationTool.Application.Services;
using Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Mvvm.Commands;

namespace DbMigration.GUI.Context;

public class ApplyMigrationCmd : AsyncDelegateCommand<ContextVm>
{
    private readonly IContextService _contextService;

    public ApplyMigrationCmd(ILogger<ApplyMigrationCmd> logger, IContextService contextService) : base(logger)
    {
        _contextService = contextService;
    }

    protected override bool CanExecuteCore(ContextVm contextVm)
    {
        if (contextVm == null || contextVm.Data.LatestMigration.IsEmpty())
            return false;

        if (contextVm.LatestAppliedMigration.IsEmpty())
            return true;

        var latestAvailable = GetDateTimeFromMigrationName(contextVm.Data.LatestMigration);
        var latestApplied = GetDateTimeFromMigrationName(contextVm.LatestAppliedMigration!);

        return 
            latestAvailable != null &&
            latestApplied != null &&
            latestApplied < latestAvailable;
    }

    private DateTime? GetDateTimeFromMigrationName(string migrationName)
    {
        string dateString = migrationName
            .Split('_')
            .First();

        return DateTime.TryParseExact(dateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
            ? dateTime
            : null;
    }

    protected override async Task ExecuteCore(ContextVm contextVm)
    {
        ApplyMigrationResult result;

        try
        {
            result = await _contextService.ApplyMigration(contextVm.Data.ContextType);
        }
        catch (Exception exc)
        {
            MessageBox.Show($"Unable to apply migrations for context '{contextVm.Data.ContextType.Name}':\n\n{exc.GetMessages()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (result == ApplyMigrationResult.Success)
            MessageBox.Show("Migration successfully applied", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        else if (result == ApplyMigrationResult.UpToDate)
            MessageBox.Show("Database already up to date", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        else if (result == ApplyMigrationResult.DataSeedToConfigStoreFailed)
            MessageBox.Show($"Unable to seed data for IdentityServer Confiugration Store", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else if (result == ApplyMigrationResult.DataSeedToUserStoreFailed)
            MessageBox.Show($"Unable to seed data for Aso user store", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        contextVm.LatestAppliedMigration = await _contextService.GetLatestAppliedMigration(contextVm.Data);
    }
}
