﻿using DbMigrationTool.Application.DTOs;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class DatabaseVm : ItemVm<DatabaseDto>
{
    public DatabaseVm(DatabaseDto data) : base(data)
    {
    }
}
