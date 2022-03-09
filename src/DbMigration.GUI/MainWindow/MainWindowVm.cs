using DbMigrationTool.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class MainWindowVm : ViewModel
{
    #region fields

    private readonly IAppService _appService;

    #endregion

    #region ctors

    public MainWindowVm(IAppService appService)
    {
        _appService = appService;
        DatabaseServers = new ObservableCollection<DatabaseServerVm>();
    }

    #endregion

    #region props

    public ObservableCollection<DatabaseServerVm> DatabaseServers { get; }

    #endregion

    #region methods

    public override async Task LoadedAsync()
    {
        var databaseServers = await _appService.GetDatabases();

        foreach (var databaseServer in databaseServers)
            DatabaseServers.Add(new DatabaseServerVm(databaseServer));
    }

    #endregion
}
