using System.ComponentModel;
using System.Windows.Controls;

namespace DbMigration.GUI.Context;

public partial class ContextView : UserControl
{
    public ContextView()
    {
        InitializeComponent();

        if (!DesignerProperties.GetIsInDesignMode(this))
            Loaded += async (s, e) => await ((ContextViewVm)DataContext).LoadedAsync();
    }
}
