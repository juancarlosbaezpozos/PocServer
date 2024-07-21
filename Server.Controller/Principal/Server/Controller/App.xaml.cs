using Principal.Controls.Core;
using Principal.Server.Agent;
using Principal.Server.Controller.Images;
using Principal.Server.Controller.NotifyIcon;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Server.Controller;

public partial class App : Application
{
    internal TaskbarIcon notifyIcon;

    private StiMenuItem menuItemRun;

    private StiMenuItem menuItemStop;

    private StiMenuItem menuItemRefresh;

    private StiMenuItem menuItemCheck;

    private StiMenuItem menuItemSettings;

    public const string AttrSecondaryRun = "-secondary";

    protected override void OnStartup(StartupEventArgs e)
    {
        if (e.Args.FirstOrDefault((string x) => x == AttrSecondaryRun) == null)
        {
            var currentId = Process.GetCurrentProcess().Id;
            if (Process.GetProcesses().FirstOrDefault((Process x) => x.ProcessName == "Principal.Server.Controller" && x.Id != currentId) != null)
            {
                Shutdown();
                return;
            }
        }
        base.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        base.OnStartup(e);
        notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
        if (notifyIcon != null)
        {
            var contextMenu = notifyIcon.ContextMenu;
            contextMenu.Opened += ContextMenu_Opened;
            menuItemRun = (StiMenuItem)contextMenu.Items[0];
            menuItemStop = (StiMenuItem)contextMenu.Items[1];
            menuItemRefresh = (StiMenuItem)contextMenu.Items[2];
            menuItemCheck = (StiMenuItem)contextMenu.Items[4];
            menuItemSettings = (StiMenuItem)contextMenu.Items[6];
        }
    }

    private void ContextMenu_Opened(object sender, RoutedEventArgs e)
    {
        var serviceStatus = StiServerController.ServiceStatus;
        menuItemRefresh.IsEnabled = serviceStatus == StiServiceStatus.Started;
        menuItemRun.IsEnabled = serviceStatus != StiServiceStatus.NotInstalled && serviceStatus == StiServiceStatus.Stopped;
        menuItemRefresh.IsEnabled = serviceStatus switch
        {
            StiServiceStatus.Started => true,
            StiServiceStatus.NotInstalled => false,
            _ => serviceStatus == StiServiceStatus.Paused,
        };
        menuItemStop.IsEnabled = serviceStatus switch
        {
            StiServiceStatus.Started => true,
            StiServiceStatus.NotInstalled => false,
            _ => serviceStatus == StiServiceStatus.Paused,
        };
        (menuItemRun.Icon as Image).Source = StiControllerImages.Items.Start();
        (menuItemRefresh.Icon as Image).Source = StiControllerImages.Items.Refresh();
        (menuItemStop.Icon as Image).Source = StiControllerImages.Items.Stop();
        (menuItemCheck.Icon as Image).Source = StiControllerImages.Items.Check();
        (menuItemSettings.Icon as Image).Source = StiControllerImages.Items.Settings();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        notifyIcon?.Dispose();
        base.OnExit(e);
    }

    [STAThread]
    public static void Main(string[] args)
    {
        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}
