using DbMigrationTool.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class DatabaseServerVm : ViewModel
{
    #region fields

    private bool _isExpanded;
    private DatabaseVm? _selectedDatabase;

    #endregion

    #region ctors

    public DatabaseServerVm(DatabaseServerDto dto)
    {
        Name = dto.Name;
        Databases = dto
            .Databases
            .Select(x => new DatabaseVm(x))
            .ToArray();
    }

    #endregion

    #region events

    public event Action<DatabaseServerVm>? DatabaseSelected;

    #endregion

    #region props 

    public string Name { get; }

    public IEnumerable<DatabaseVm> Databases { get; }

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
}
