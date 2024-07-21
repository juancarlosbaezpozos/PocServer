using Principal.Server.Objects;
using System.Threading.Tasks;

namespace Principal.Server.Agent;

public static class StiServerController
{
    public static StiCore Core { get; private set; }

    public static StiServiceStatus ServiceStatus
    {
        get
        {
            if (IsWindowsServiceMode || Core == null)
            {
                return StiServerAgentHelper.ServiceStatus;
            }
            return Core.GetStatus() switch
            {
                StiProcessorStatus.Paused => StiServiceStatus.Paused,
                StiProcessorStatus.Started => StiServiceStatus.Started,
                StiProcessorStatus.Stopped => StiServiceStatus.Stopped,
                _ => StiServiceStatus.NotInstalled,
            };
        }
    }

    public static bool IsWindowsServiceMode => ServerMode == StiServerMode.WindowsService;

    public static bool IsThreadMode => ServerMode == StiServerMode.Threads;

    public static StiServerMode ServerMode { get; set; }

    public static async Task InstallServiceAsync()
    {
        await Task.Run(InstallService);
    }

    public static void InstallService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.InstallService();
        }
        else
        {
            Core = new StiCore();
        }
    }

    public static async Task UninstallServiceAsync()
    {
        await Task.Run(UninstallService);
    }

    public static void UninstallService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.UninstallService();
        }
        else
        {
            Core = null;
        }
    }

    public static async Task StartServiceAsync()
    {
        await Task.Run(StartService);
    }

    public static void StartService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.StartService();
        }
        else
        {
            Core.Start();
        }
    }

    public static async Task RestartServiceAsync()
    {
        await Task.Run(RestartService);
    }

    public static void RestartService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.RestartService();
        }
        else
        {
            Core.Restart();
        }
    }

    public static async Task PauseServiceAsync()
    {
        await Task.Run(PauseService);
    }

    public static void PauseService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.PauseService();
        }
        else
        {
            Core.Pause();
        }
    }

    public static async Task ResumeServiceAsync()
    {
        await Task.Run(ResumeService);
    }

    public static void ResumeService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.ResumeService();
        }
        else
        {
            Core.Resume();
        }
    }

    public static async Task StopServiceAsync()
    {
        await Task.Run(StopService);
    }

    public static void StopService()
    {
        if (IsWindowsServiceMode)
        {
            StiServerAgentHelper.StopService();
        }
        else
        {
            Core.Stop();
        }
    }
}
