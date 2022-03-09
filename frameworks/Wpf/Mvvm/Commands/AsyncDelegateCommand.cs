using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Wpf.Mvvm.Commands
{
    public sealed class AsyncDelegateCommand : DelegateCommand, IAsyncDelegateCommand
    {
        #region Fields

        private bool _isRunning;
        private readonly Func<Task> _task;
        private readonly Func<bool> _canExecute;

        #endregion

        #region ctors

        public AsyncDelegateCommand(Func<Task> execute, ILogger logger) : this(execute, null, logger) { }

        public AsyncDelegateCommand(Func<Task> task, Func<bool> canExecute, ILogger logger) : base(logger)
        {
            _task = task ?? throw new ArgumentException(nameof(task));
            _canExecute = canExecute;
        }

        #endregion

        #region events

        public event Action ExecutionStarted;
        public event Action ExecutionCompleted;

        #endregion

        #region methods

        public override sealed bool CanExecute(object parameter) =>
            _canExecute?.Invoke() ?? true;

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
                await _task();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Unhandled exception during command execution");
                throw;
            }
            finally
            {
                _isRunning = false;
                ExecutionCompleted?.Invoke();
            }
        }


        #endregion
    }
}
