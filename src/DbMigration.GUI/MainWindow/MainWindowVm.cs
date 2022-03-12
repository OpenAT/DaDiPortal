using DbMigration.GUI.Context;
using DbMigrationTool.Application.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class MainWindowVm : ViewModel
{
    #region fields

    private readonly IAppService _appService;
    private DatabaseVm? _selectedDatabase;

    #endregion

    #region ctors

    public MainWindowVm(IAppService appService, ContextViewVm contextViewVm)
    {
        DatabaseServers = new ObservableCollection<DatabaseServerVm>();

        _appService = appService;
        ContextViewVm = contextViewVm;
    }

    #endregion

    #region props

    public ObservableCollection<DatabaseServerVm> DatabaseServers { get; }

    public ContextViewVm ContextViewVm { get; }

    public DatabaseVm? SelectedDatabase
    {
        get { return _selectedDatabase; }
        set 
        { 
            _selectedDatabase = value;
            RaiseChanged();

            if (_selectedDatabase != null)
                _ = ContextViewVm.DatabaseSelected(_selectedDatabase.Server, _selectedDatabase.Data);
        }
    }


    #endregion

    #region methods

    public override async Task LoadedAsync()
    {
        ShowBusyCursor();
        var databaseServers = await _appService.GetServers();
        ShowDefaultCursor();

        foreach (var databaseServer in databaseServers)
        {
            var databaseServerVm = new DatabaseServerVm(databaseServer, _appService);
            DatabaseServers.Add(databaseServerVm);
        }
    }

    #endregion
}
