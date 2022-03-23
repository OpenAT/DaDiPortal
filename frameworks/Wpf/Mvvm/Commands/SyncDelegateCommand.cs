using Microsoft.Extensions.Logging;
using System;

namespace Wpf.Mvvm.Commands
{
    public sealed class SyncDelegateCommand : DelegateCommand, ISyncDelegateCommand
    {
        #region Fields

        private Action _execute;
        private Func<bool> _canExecute;

        #endregion

        #region ctors

        public SyncDelegateCommand(Action execute, ILogger logger) : this(execute, null, logger) { }

        public SyncDelegateCommand(Action execute, Func<bool> canExecute, ILogger logger) : base(logger)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region methods

        public override sealed bool CanExecute(object parameter) =>
            _canExecute?.Invoke() ?? true;

        public override sealed void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _execute();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Unhandled exception during command execution");
                throw;
            }
        }

        #endregion
    }
}
