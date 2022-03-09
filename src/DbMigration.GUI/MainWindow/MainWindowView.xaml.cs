using System.Windows;

namespace DbMigration.GUI.MainWindow
{
    public partial class MainWindowView : Window
    {
        public MainWindowView(MainWindowVm viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            Loaded += async (s, e) => 
                await viewModel.LoadedAsync();
        }
    }
}
