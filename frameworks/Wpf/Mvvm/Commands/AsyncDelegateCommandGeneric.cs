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
            _logger.LogInformation($"Executing command {GetType().Name}");

            if (_isRunning)
            {
                _logger.LogWarning($"Cannot execute command {GetType().Name} because it is already running");
                return;
            }

            if (!CanExecute(parameter))
            {
                _logger.LogWarning($"Cannot execute command {GetType().Name} because CanExecute returned false");
                return;
            }


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
                _logger.LogInformation($"Execution of command {GetType().Name} completed");
            }
        }

        protected virtual bool CanExecuteCore(T parameter) => true;

        protected abstract Task ExecuteCore(T parameter);

        #endregion
    }
}
