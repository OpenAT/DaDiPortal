using Microsoft.Extensions.Logging;
using Wpf.Mvvm.Commands;

namespace Wpf.Mvvm.ViewModels
{
    public interface IDialogViewModel : IWindowViewModel
    {
        void SetData(object data);
    }

    public abstract class DialogViewModel<T> : WindowViewModel, IDialogViewModel
    {
        #region fields

        protected T _data;

        #endregion

        #region ctors

        protected DialogViewModel(ILogger logger) : base(logger)
        {
            SaveCmd = new SyncDelegateCommand(Save, CanSave, logger);
            CancelCmd = new SyncDelegateCommand(Cancel, CanCancel, logger);
        }

        #endregion

        #region SaveCmd

        public ISyncDelegateCommand SaveCmd { get; }
        protected virtual bool CanSave() => true;
        protected virtual void Save() { }

        #endregion

        #region CancelCmd

        public ISyncDelegateCommand CancelCmd { get; }
        protected virtual bool CanCancel() => true;
        protected virtual void Cancel() { }

        #endregion

        #region methods

        public void SetData(object data) =>
            _data = (T)data;

        #endregion
    }
}
