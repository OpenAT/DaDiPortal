using DbMigrationTool.Application.DTOs;
using System.Windows.Input;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.Context;

public class ContextVm : ItemVm<ContextDto>
{
    #region ctors

    public ContextVm(ContextDto data) : base(data)
    {
    }

    #endregion

    #region props 

    public string? LatestAppliedMigration
    {
        get => Data.LatestAppliedMigration;
        set
        {
            Data.LatestAppliedMigration = value;
            RaiseChanged();
        }
    }

    #endregion
}
