using DbMigrationTool.Application.DTOs;
using Wpf.Mvvm.ViewModels;

namespace DbMigration.GUI.MainWindow;

public class DatabaseVm : ItemVm<string>
{
    public DatabaseVm(string databaseName, string server) : base(databaseName)
    {
        Server = server;
    }

    public string Server { get; }
}
