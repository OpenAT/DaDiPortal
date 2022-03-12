using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf.Mvvm.Commands
{
    public abstract class AsyncDelegateCommand<T> : DelegateCommand, IAsyncDelegateCommand<T>
    {
        #region Fields

        private bool _isRunning;

        #endregion

        #region ctors

        public AsyncDelegateCommand(ILogger logger) : base(logger) { }

        #endregion

        #region events

        public event Action ExecutionStarted;
        public event Action ExecutionCompleted;

        #endregion

        #region methods

        public override sealed bool CanExecute(object parameter) =>
            CanExecuteCore((T)parameter);

        public async override sealed void Execute(object parameter)
        {
            if (_isRunning)
                return;

            if (!CanExecute(parameter))
                return;


            _isRunning = true;
            ExecutionStarted?.Invoke();

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await ExecuteCore((T)parameter);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Unhandled exception during command execution");
                throw;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                _isRunning = false;
                ExecutionCompleted?.Invoke();
            }
        }

        protected virtual bool CanExecuteCore(T parameter) => true;

        protected abstract Task ExecuteCore(T parameter);

        #endregion
    }
}
