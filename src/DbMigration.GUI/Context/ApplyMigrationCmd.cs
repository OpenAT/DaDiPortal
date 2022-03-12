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

        string dateStrOfLatestAvailable = contextVm
            .Data
            .LatestMigration
            .Split('_')
            .First();

        string dateStrOfLatestApplied = contextVm
            .LatestAppliedMigration!
            .Split('_')
            .First();

        if (!DateTime.TryParseExact(dateStrOfLatestAvailable, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeLatest))
            return false;

        if (!DateTime.TryParseExact(dateStrOfLatestApplied, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeLatestApplied))
            return false;

        return dateTimeLatest > dateTimeLatestApplied;
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
            MessageBox.Show($"Unable to apply migrations for context '{contextVm.Data.ContextType.Name}':\n\n{exc.GetMessages()}");
            return;
        }

        if (result == ApplyMigrationResult.Success)
            MessageBox.Show("Migration successfully applied");
        else if (result == ApplyMigrationResult.UpToDate)
            MessageBox.Show("Database already up to date");
    }
}
