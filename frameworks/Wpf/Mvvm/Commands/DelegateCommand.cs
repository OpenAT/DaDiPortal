using Microsoft.Extensions.Logging;
using System;

namespace Wpf.Mvvm.Commands
{
    public abstract class DelegateCommand : IDelegateCommand
    {
        #region fields

        protected readonly ILogger _logger;

        #endregion

        #region ctors

        protected DelegateCommand(ILogger logger) =>
            _logger = logger;

        #endregion

        #region events

        public event EventHandler CanExecuteChanged;

        #endregion

        #region methods

        public virtual bool CanExecute(object parameter) => true;

        public abstract void Execute(object parameter);

        public void Recheck()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
