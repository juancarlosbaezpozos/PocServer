using Principal.Server.Agent;
using Principal.Server.Controller.NotifyIcon;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Principal.Server.Controller;

public class NotifyIconViewModel
{
    internal static StiManagerWindow ManagerWindow { get; set; }

    public ICommand StartServerCommand => new AsyncDelegateCommand(async delegate
    {
        if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
        {
            if (taskbarIcon.ContextMenu != null)
                taskbarIcon.ContextMenu.IsOpen = false;
        }
        await StiServerController.StartServiceAsync();
    });

    public ICommand StopServerCommand => new AsyncDelegateCommand(async delegate
    {
        if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
        {
            if (taskbarIcon.ContextMenu != null)
                taskbarIcon.ContextMenu.IsOpen = false;
        }
        await StiServerController.StopServiceAsync();
    });

    public ICommand CheckServerCommand => new AsyncDelegateCommand(async delegate
    {
        if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
        {
            if (taskbarIcon.ContextMenu != null)
                taskbarIcon.ContextMenu.IsOpen = false;
        }

        await Task.Delay(100);

        try
        {
            if (ManagerWindow == null)
            {
                ManagerWindow = new StiManagerWindow();
                ManagerWindow.Show();
            }
            else
            {
                ManagerWindow.Activate();
            }
            ManagerWindow.serverControl.tabControl.SelectedItem = ManagerWindow.serverControl.pageCheck;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
    });

    public ICommand SettingsCommand => new AsyncDelegateCommand(async delegate
    {
        if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
        {
            if (taskbarIcon.ContextMenu != null)
                taskbarIcon.ContextMenu.IsOpen = false;
        }

        await Task.Delay(100);

        try
        {
            if (ManagerWindow == null)
            {
                ManagerWindow = new StiManagerWindow();
                ManagerWindow.Closed += delegate
                {
                    ManagerWindow = null;
                };
                ManagerWindow.Show();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    ManagerWindow.Activate();
                    ManagerWindow.Topmost = true;
                });
            }

            ManagerWindow.serverControl.tabControl.SelectedItem = ManagerWindow.serverControl.pageWindowsServices;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
    });

    public ICommand RestartServerCommand => new AsyncDelegateCommand(async delegate
    {
        if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
        {
            if (taskbarIcon.ContextMenu != null)
                taskbarIcon.ContextMenu.IsOpen = false;
        }
        await StiServerController.RestartServiceAsync();
    });

    public ICommand ExitApplicationCommand => new AsyncDelegateCommand(async delegate
    {
        try
        {
            if (Application.Current.FindResource("NotifyIcon") is TaskbarIcon taskbarIcon)
            {
                if (taskbarIcon.ContextMenu != null)
                    taskbarIcon.ContextMenu.IsOpen = false;
            }
            await Task.Delay(5);
        }
        catch
        {
        }
        Application.Current.Shutdown();
    });

}
