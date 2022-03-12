using DbMigrationTool.Application.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class MainWindowVm : ViewModel
{
    #region fields

    private readonly IAppService _appService;
    private readonly IConnectionStringProvider _connectionStringProvider;

    #endregion

    #region ctors

    public MainWindowVm(
        IAppService appService, 
        IConnectionStringProvider connectionStringProvider)
    {
        DatabaseServers = new ObservableCollection<DatabaseServerVm>();
        Contexts = new ObservableCollection<ContextVm>();

        _appService = appService;
        _connectionStringProvider = connectionStringProvider;
    }

    #endregion

    #region props

    public ObservableCollection<DatabaseServerVm> DatabaseServers { get; }
    public ObservableCollection<ContextVm> Contexts { get; }

    #endregion

    #region methods

    public override async Task LoadedAsync()
    {
        ShowBusyCursor();
        var databaseServers = await _appService.GetDatabases();
        ShowDefaultCursor();

        foreach (var databaseServer in databaseServers)
        {
            var databaseServerVm = new DatabaseServerVm(databaseServer);
            databaseServerVm.DatabaseSelected += OnDatabaseSelected;

            DatabaseServers.Add(databaseServerVm);
        }
    }

    #endregion

    #region event handlers

    private async void OnDatabaseSelected(DatabaseServerVm databaseServerVm)
    {
        foreach (var dbServer in DatabaseServers)
            if (dbServer != databaseServerVm)
                dbServer.SelectedDatabase = null;

        _connectionStringProvider.SetConnection(databaseServerVm.Name, databaseServerVm.SelectedDatabase!.Data.Name);

        ShowBusyCursor();
        var contexts = await _appService.GetContexts();
        ShowDefaultCursor();

        Contexts.Clear();
        foreach (var context in contexts)
        {
            var contextVm = new ContextVm(context);
            Contexts.Add(contextVm);
        }
    }

    #endregion
}
