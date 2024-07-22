using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using Principal.Server.Agent;
using Principal.Server.Controller.Images;
using Principal.Server.Objects;

namespace Principal.Server.Controller.Controls;

public partial class StiServiceInstallationControl : UserControl, IComponentConnector
{
    private readonly DispatcherTimer timerStatus;

    private bool LockThreadCount { get; set; }

    public bool IsWorking { get; private set; }

    public StiServiceInstallationControl()
    {
        LockThreadCount = true;
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            timerStatus = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1)
            };
            timerStatus.Tick += TimerStatusTick;
            timerStatus.Start();
            ProcessAdministratorWarning();
            RefreshServiceStatus();
            //comboBoxThreadCount.SelectedIndex = 3;
            DoScaling();
            LockThreadCount = false;
        }
    }

    private bool ProcessAdministratorWarning()
    {
        try
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                panelRunAsAdministrator.Visibility = Visibility.Visible;
                return true;
            }
            panelRunAsAdministrator.Visibility = Visibility.Collapsed;
        }
        catch
        {
        }
        return false;
    }

    private void DoScaling()
    {
        imageThreadRestart.Source = StiControllerImages.Items.Warning(StiImageSize.Double);
        imageRunAsAdministratorWarning.Source = StiControllerImages.Items.Warning(StiImageSize.Double);
        imageProcessorError.Source = StiControllerImages.Items.Error(StiImageSize.Double);
    }

    private void SetInstallError(string error)
    {
        textErrorInstall.Text = error;
        textErrorInstall.Visibility = ((error == null) ? Visibility.Collapsed : Visibility.Visible);
    }

    private void RefreshServiceStatus()
    {
        try
        {
            StiServiceStatus serviceStatus = StiServerController.ServiceStatus;
            if (serviceStatus != StiServiceStatus.NotInstalled)
            {
                textInstalled.FontWeight = FontWeights.Bold;
                textInstalled.Text = "Instalado";
            }
            else
            {
                textInstalled.FontWeight = FontWeights.Regular;
                textInstalled.Text = "No Instalado";
            }
            switch (serviceStatus)
            {
                case StiServiceStatus.Started:
                    textStarted.Text = "Iniciado";
                    break;
                case StiServiceStatus.Starting:
                    textStarted.Text = "Iniciando...";
                    break;
                default:
                    textStarted.Text = string.Empty;
                    break;
            }
            switch (serviceStatus)
            {
                case StiServiceStatus.Stopped:
                    textStopped.Text = "Detenido";
                    break;
                case StiServiceStatus.Stopping:
                    textStopped.Text = "Deteniendo...";
                    break;
                default:
                    textStopped.Text = string.Empty;
                    break;
            }
            buttonRestart.IsEnabled = serviceStatus == StiServiceStatus.Started;
            buttonInstall.IsEnabled = !IsWorking && serviceStatus == StiServiceStatus.NotInstalled;
            buttonUninstall.IsEnabled = !IsWorking && serviceStatus != StiServiceStatus.NotInstalled;
            buttonStart.IsEnabled = !IsWorking && serviceStatus != StiServiceStatus.NotInstalled && serviceStatus == StiServiceStatus.Stopped;
            buttonRestart.IsEnabled = !IsWorking && serviceStatus != StiServiceStatus.NotInstalled && (serviceStatus == StiServiceStatus.Started || serviceStatus == StiServiceStatus.Paused);
            buttonStop.IsEnabled = !IsWorking && serviceStatus != StiServiceStatus.NotInstalled && (serviceStatus == StiServiceStatus.Started || serviceStatus == StiServiceStatus.Paused);
            panelProcessorError.Visibility = Visibility.Collapsed;
        }
        catch
        {
        }
    }

    private void SetStatusError(string error)
    {
        textErrorStatus.Text = error;
        textErrorStatus.Visibility = ((error == null) ? Visibility.Collapsed : Visibility.Visible);
    }

    internal async Task InvokeRestartServiceAsync()
    {
        _ = 1;

        try
        {
            IsWorking = true;
            SetStatusError(null);
            progressRestart.IsActive = true;
            await Task.Delay(10);
            await StiServerController.RestartServiceAsync();
            RefreshServiceStatus();
        }
        catch (Exception ex)
        {
            SetStatusError(ex.Message);
        }
        finally
        {
            progressRestart.IsActive = false;
            IsWorking = false;
        }
    }

    private async Task StopServiceAsync()
    {
        _ = 1;

        try
        {
            IsWorking = true;
            SetStatusError(null);
            progressStop.IsActive = true;
            await Task.Delay(10);
            await StiServerController.StopServiceAsync();
            RefreshServiceStatus();
        }
        catch (Exception ex)
        {
            SetStatusError(ex.Message);
        }
        finally
        {
            progressStop.IsActive = false;
            IsWorking = false;
        }
    }

    private async Task StartServiceAsync()
    {
        _ = 1;

        try
        {
            IsWorking = true;
            SetStatusError(null);
            progressStart.IsActive = true;
            await Task.Delay(10);
            await StiServerController.StartServiceAsync();
            RefreshServiceStatus();
        }
        catch (Exception ex)
        {
            SetStatusError(ex.Message);
        }
        finally
        {
            progressStart.IsActive = false;
            IsWorking = false;
        }
    }

    private async Task UninstallServiceAsync()
    {
        _ = 2;

        try
        {
            IsWorking = true;
            SetInstallError(null);
            progressUnistall.IsActive = true;
            await Task.Delay(10);
            await StiServerController.StopServiceAsync();
            RefreshServiceStatus();
            textInstalled.Text = "Desinstalando...";
            await StiServerController.UninstallServiceAsync();
            RefreshServiceStatus();
        }
        catch (Exception ex)
        {
            SetInstallError(ex.Message);
        }
        finally
        {
            progressUnistall.IsActive = false;
            IsWorking = false;
        }
    }

    private async Task InstallServiceAsync()
    {
        _ = 1;

        try
        {
            IsWorking = true;
            SetInstallError(null);
            progressInstall.IsActive = true;
            await Task.Delay(10);
            textInstalled.Text = "Instalando...";
            await StiServerController.InstallServiceAsync();
            RefreshServiceStatus();
        }
        catch (Exception ex)
        {
            SetInstallError(ex.Message);
        }
        finally
        {
            progressInstall.IsActive = false;
            IsWorking = false;
        }
    }

    private void TimerStatusTick(object sender, EventArgs e)
    {
        RefreshServiceStatus();
    }

    private async void ButtonInstallClick(object sender, RoutedEventArgs e)
    {
        await InstallServiceAsync();
    }

    private async void ButtonUninstallClick(object sender, RoutedEventArgs e)
    {
        await UninstallServiceAsync();
    }

    private async void ButtonStartClick(object sender, RoutedEventArgs e)
    {
        panelTreadRestart.Visibility = Visibility.Collapsed;
        await StartServiceAsync();
    }

    private async void ButtonRestartClick(object sender, RoutedEventArgs e)
    {
        panelTreadRestart.Visibility = Visibility.Collapsed;
        await InvokeRestartServiceAsync();
    }

    private async void ButtonStopClick(object sender, RoutedEventArgs e)
    {
        await StopServiceAsync();
    }

    private void OnRunAsAdministratorClick(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Assembly.GetEntryAssembly().Location,
                Verb = "runas",
                Arguments = "-secondary"
            });
            Application.Current.Shutdown();
        }
        catch
        {
        }
    }

    //private void ComboBoxThreadCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (!LockThreadCount)
    //    {
    //        if (IsWorking)
    //        {
    //            panelTreadRestart.Visibility = Visibility.Visible;
    //        }
    //    }
    //}
}
