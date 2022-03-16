using DbMigration.GUI.MainWindow;
using DbMigrationTool.Application.Services;
using Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Mvvm.Commands;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.Context;

public class ContextViewVm : ViewModel
{
    #region fields

    private readonly IContextService _contextService;
    private readonly ILogger _logger;

    #endregion

    #region ctors

    public ContextViewVm(IContextService contextService, ApplyMigrationCmd applyMigrationCmd, ILogger<ContextViewVm> logger)
    {
        _contextService = contextService;
        ApplyMigrationCmd = applyMigrationCmd;
        Contexts = new ObservableCollection<ContextVm>();
        _logger = logger;
    }

    #endregion

    #region cmds

    public ApplyMigrationCmd ApplyMigrationCmd { get; }

    #endregion

    #region props 

    public ObservableCollection<ContextVm> Contexts { get; }

    #endregion

    #region methods

    public override async Task LoadedAsync()
    {
        ShowBusyCursor();
        var contexts = await _contextService.GetContexts();
        ShowDefaultCursor();

        foreach (var context in contexts)
        {
            var contextVm = new ContextVm(context);
            Contexts.Add(contextVm);
        }
    }

    public async Task DatabaseSelected(string server, string database)
    {
        _logger.LogInformation($"Database selected: {server}/{database}");

        ShowBusyCursor();
        _contextService.SetConnection(server, database);

        try
        {
            foreach (var contextVm in Contexts)
                contextVm.LatestAppliedMigration = await _contextService.GetLatestAppliedMigration(contextVm.Data);
        }
        catch (Exception exc)
        {
            MessageBox.Show($"Unable to get latest applied migrations:\n\n{exc.GetMessages()}");
        }

        ShowDefaultCursor();
        ApplyMigrationCmd.Recheck();
    }

    #endregion
}
