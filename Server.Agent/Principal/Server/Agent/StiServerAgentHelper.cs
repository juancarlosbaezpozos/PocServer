using System;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Principal.Server.Agent;

public static class StiServerAgentHelper
{
    private const int Timeout = 10;

    private static string ApplicationPath => typeof(StiServerAgentHelper).Assembly.Location;

    public static StiServiceStatus ServiceStatus
    {
        get
        {
            if (!IsServiceInstalled())
            {
                return StiServiceStatus.NotInstalled;
            }
            try
            {
                using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);
                if (serviceController.Status == ServiceControllerStatus.ContinuePending)
                {
                    return StiServiceStatus.Resuming;
                }
                if (serviceController.Status == ServiceControllerStatus.Paused)
                {
                    return StiServiceStatus.Paused;
                }
                if (serviceController.Status == ServiceControllerStatus.PausePending)
                {
                    return StiServiceStatus.Pausing;
                }
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    return StiServiceStatus.Started;
                }
                if (serviceController.Status == ServiceControllerStatus.StartPending)
                {
                    return StiServiceStatus.Starting;
                }
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    return StiServiceStatus.Stopped;
                }
                if (serviceController.Status == ServiceControllerStatus.StopPending)
                {
                    return StiServiceStatus.Stopping;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return StiServiceStatus.NotInstalled;
        }
    }

    public static bool InstallService()
    {
        if (IsServiceInstalled())
        {
            UninstallService();
        }
        var assemblyInstaller = new AssemblyInstaller(ApplicationPath, new string[1] { "/logtoconsole=false" });
        assemblyInstaller.UseNewContext = true;
        assemblyInstaller.Install(null);
        assemblyInstaller.Commit(null);
        if (ServiceStatus == StiServiceStatus.NotInstalled)
        {
            return false;
        }
        return true;
    }

    public static bool UninstallService()
    {
        try
        {
            if (ServiceStatus != StiServiceStatus.NotInstalled)
            {
                var assemblyInstaller = new AssemblyInstaller(ApplicationPath, new string[1] { "/logtoconsole=false" });
                assemblyInstaller.UseNewContext = true;
                assemblyInstaller.Uninstall(null);
                if (ServiceStatus == StiServiceStatus.NotInstalled)
                {
                    return true;
                }
                return false;
            }
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }
        return true;
    }

    public static bool ExecuteCommand(StiCoreAgentCommand command)
    {
        if (!IsServiceInstalled())
        {
            return false;
        }

        using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);

        try
        {
            if (serviceController.Status != ServiceControllerStatus.Running)
            {
                return false;
            }

            serviceController.ExecuteCommand((int)command);
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }

        return true;
    }

    public static bool StartService()
    {
        using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);

        try
        {
            serviceController.Start();
            serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(Timeout));
            return true;
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }
        finally
        {
            if (ServiceStatus == StiServiceStatus.Started)
            {
                System.Diagnostics.Debug.WriteLine("hecho");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("fallido");
            }
        }
    }

    public static bool RestartService()
    {
        try
        {
            StopService();
        }
        catch
        {
        }
        return StartService();
    }

    public static bool PauseService()
    {
        using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);
        try
        {
            serviceController.Pause();
            serviceController.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(10.0));
            return true;
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }
        finally
        {
            if (ServiceStatus == StiServiceStatus.Started)
            {
                System.Diagnostics.Debug.WriteLine("hecho");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("fallido");
            }
        }
    }

    public static bool ResumeService()
    {
        using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);
        try
        {
            serviceController.Continue();
            serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10.0));
            return true;
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }
        finally
        {
            if (ServiceStatus == StiServiceStatus.Started)
            {
                System.Diagnostics.Debug.WriteLine("hecho");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("fallido");
            }
        }
    }

    public static bool StopService()
    {
        if (ServiceStatus != StiServiceStatus.Started && ServiceStatus != StiServiceStatus.Starting && ServiceStatus != StiServiceStatus.Paused && ServiceStatus != StiServiceStatus.Pausing)
        {
            return true;
        }
        using var serviceController = new ServiceController(StiServerAgent.ServerAgentName);
        try
        {
            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10.0));
            return true;
        }
        catch (Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(exception);
            return false;
        }
        finally
        {
            if (ServiceStatus == StiServiceStatus.Started)
            {
                System.Diagnostics.Debug.WriteLine("hecho");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("fallido");
            }
        }
    }

    public static bool IsServiceInstalled()
    {
        return ServiceController.GetServices().Any((ServiceController service) => service.ServiceName == StiServerAgent.ServerAgentName);
    }
}
