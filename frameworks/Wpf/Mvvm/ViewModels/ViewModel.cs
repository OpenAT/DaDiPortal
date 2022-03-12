using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf.Mvvm.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
    }

    public abstract class ViewModel : IViewModel
    {
        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region methods

        public virtual Task LoadedAsync()
            => Task.CompletedTask;

        protected void RaiseChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        protected void ShowBusyCursor()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        protected void ShowDefaultCursor()
        {
            Mouse.OverrideCursor = null;
        }

        #endregion
    }
}
