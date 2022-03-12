using DbMigration.GUI.MainWindow;
using DbMigrationTool.Application.Services;
using Extensions;
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

    #endregion

    #region ctors

    public ContextViewVm(IContextService contextService, ApplyMigrationCmd applyMigrationCmd)
    {
        _contextService = contextService;
        ApplyMigrationCmd = applyMigrationCmd; 
        Contexts = new ObservableCollection<ContextVm>();
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
        ApplyMigrationCmd.CheckConditions();
    }

    #endregion
}
