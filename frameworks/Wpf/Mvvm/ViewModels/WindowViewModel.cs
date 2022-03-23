using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Mvvm.Commands;

namespace Wpf.Mvvm.ViewModels
{
    public interface IWindowViewModel : IViewModel
    {
        IAsyncDelegateCommand LoadCmd { get; }
    }

    public abstract class WindowViewModel : ViewModel, IWindowViewModel
    {
        #region fields

        private int _runningCommands;
        protected readonly ILogger _logger;

        #endregion

        #region ctors

        public WindowViewModel(ILogger logger)
        {
            _logger = logger;
            LoadCmd = new AsyncDelegateCommand(LoadTask, logger);
        }

        #endregion

        #region props

        public Visibility BusyIndicatorVisibility
        {
            get => _runningCommands > 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        #endregion

        #region commands

        public IAsyncDelegateCommand LoadCmd { get; }

        #endregion

        #region methods

        protected virtual Task LoadTask() => Task.CompletedTask;

        protected void AddCommandToStateSurveillance(params IAsyncDelegateCommand[] commands)
        {
            foreach (var command in commands)
            {
                command.ExecutionStarted += OnCommandExecutionStarted;
                command.ExecutionCompleted += OnCommandExecutionCompleted;
            }
        }

        #endregion

        #region event handlers

        private void OnCommandExecutionCompleted()
        {
            _runningCommands--;
            RaiseChanged(nameof(BusyIndicatorVisibility));
        }

        private void OnCommandExecutionStarted()
        {
            _runningCommands++;
            RaiseChanged(nameof(BusyIndicatorVisibility));
        }

        #endregion
    }
}
