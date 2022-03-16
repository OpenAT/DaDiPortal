using System;
using System.Windows.Input;

namespace Wpf.Mvvm.Commands
{
    public interface IDelegateCommand : ICommand
    {
        void Recheck();
    }

    public interface ISyncDelegateCommand : IDelegateCommand
    {
    }

    public interface IAsyncDelegateCommand : IDelegateCommand
    {
        event Action ExecutionStarted;
        event Action ExecutionCompleted;
    }

    public interface IAsyncDelegateCommand<T> : IAsyncDelegateCommand
    {
    }
}
