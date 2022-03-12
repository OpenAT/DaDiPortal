using DbMigrationTool.Application.DTOs;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class ContextVm : ItemVm<ContextDto>
{
    public ContextVm(ContextDto data) : base(data)
    {
    }
}
