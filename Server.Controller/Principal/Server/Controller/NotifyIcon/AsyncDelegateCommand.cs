using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Principal.Server.Controller.NotifyIcon;

public class AsyncDelegateCommand : ICommand
{
    private readonly Predicate<object> canExecute;

    private readonly Func<object, Task> asyncExecute;

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    public async void Execute(object parameter)
    {
        await ExecuteAsync(parameter);
    }

    protected virtual async Task ExecuteAsync(object parameter)
    {
        await asyncExecute(parameter);
    }

    public AsyncDelegateCommand(Func<object, Task> execute)
        : this(execute, null)
    {
    }

    public AsyncDelegateCommand(Func<object, Task> asyncExecute, Predicate<object> canExecute)
    {
        this.asyncExecute = asyncExecute;
        this.canExecute = canExecute;
    }
}
