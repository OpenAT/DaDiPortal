using System.Windows;
using Wpf.Mvvm.ViewModels;

namespace Wpf.Dialogs
{
    public interface IDialog
    {
        IDialogViewModel ViewModel { get; }

        bool? ShowDialog();
    }

    public abstract class Dialog : Window, IDialog
    {
        public IDialogViewModel ViewModel
        {
            get => (IDialogViewModel)DataContext;
        }
    }
}
