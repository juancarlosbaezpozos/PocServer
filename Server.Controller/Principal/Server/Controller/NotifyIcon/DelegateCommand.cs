using System;
using System.Windows.Input;

namespace Principal.Server.Controller.NotifyIcon;

public class DelegateCommand : ICommand
{
    public Action CommandAction { get; set; }

    public Func<bool> CanExecuteFunc { get; set; }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object parameter)
    {
        CommandAction();
    }

    public bool CanExecute(object parameter)
    {
        return CanExecuteFunc == null || CanExecuteFunc();
    }
}
