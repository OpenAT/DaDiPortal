using DbMigrationTool.Application.Services;
using Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class DatabaseServerVm : ItemVm<string>
{
    #region fields

    private bool _isExpanded;
    private DatabaseVm? _selectedDatabase;
    private readonly IAppService _appService;

    #endregion

    #region ctors

    public DatabaseServerVm(string name, IAppService appService) : base(name)
    {
        _appService = appService;
        Databases = new ObservableCollection<DatabaseVm>();
    }

    #endregion

    #region events

    public event Action<DatabaseServerVm>? DatabaseSelected;

    #endregion

    #region props 

    public ObservableCollection<DatabaseVm> Databases { get; }

    public DatabaseVm? SelectedDatabase
    {
        get { return _selectedDatabase; }
        set 
        { 
            _selectedDatabase = value;
            RaiseChanged();

            if (_selectedDatabase != null)
                DatabaseSelected?.Invoke(this);
        }
    }


    public bool IsExpanded
    {
        get { return _isExpanded; }
        set 
        { 
            _isExpanded = value;
            RaiseChanged();
        }
    }

    #endregion

    #region methods

    protected override async Task OnSelected()
    {
        if (Databases.Any())
            return;


        ShowBusyCursor();

        try
        {
            var databases = await _appService.GetDatabases(Data);

            foreach (var database in databases)
                Databases.Add(new DatabaseVm(database, Data));
        }
        catch (Exception exc)
        {
            MessageBox.Show($"Unable to get databases from server '{Data}': {exc.GetMessages()}");
        }
        finally
        {
            ShowDefaultCursor();
        }
    }

    #endregion
}
