using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Principal.Server.Agent;

public sealed class StiServerAgent : ServiceBase
{
    public const string ServerAgentName = "PrincipalServerAgent";

    public const string DisplayAgentName = "Principal Server Agent";

    public const string ServerAgentDescription = "Principal Server Agent. Este servicio proveé funcionalidad core a Principal Server.";

    public StiCore Core { get; private set; }

    protected override void OnStart(string[] args)
    {
        //#if DEBUG
        //        base.RequestAdditionalTime(600 * 1000);   //10 minutos
        //        Debugger.Launch();
        //#endif

        Core = new StiCore();
        Core.Start();

        base.OnStart(args);
    }

    protected override void OnStop()
    {
        Core.Stop();
        Core = null;
        ExitCode = 0;

        base.OnStop();
    }

    protected override void OnPause()
    {
        Core.Pause();
    }

    protected override void OnContinue()
    {
        Core.Resume();
    }

    private static void Main()
    {
        var servicesToRun = new ServiceBase[]
        {
            new StiServerAgent()
        };

        if (Environment.UserInteractive)
        {
            RunInteractive(servicesToRun);
        }
        else
        {
            Run(servicesToRun);
        }
    }

    public StiServerAgent()
    {
        ServiceName = ServerAgentName;
        EventLog.Log = "Application";
        CanHandlePowerEvent = true;
        CanHandleSessionChangeEvent = true;
        CanPauseAndContinue = true;
        CanShutdown = true;
        CanStop = true;
    }

    private static void RunInteractive(ServiceBase[] servicesToRun)
    {
        Debug.WriteLine("Servicios corriendo en modo interactivo.");

        var onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
            BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var service in servicesToRun)
        {
            Debug.WriteLine("\nIniciando {0}...", service.ServiceName);
            onStartMethod?.Invoke(service, new object[] { new string[] { } });
            Debug.WriteLine("\nIniciado");
        }

        Debug.WriteLine(
            "\nPresione cualquier tecla para detener el servicio y terminar el proceso...");
        Console.ReadKey();

        var onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
            BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var service in servicesToRun)
        {
            Debug.WriteLine("\nDeteniendo {0}...", service.ServiceName);
            onStopMethod?.Invoke(service, null);
            Debug.WriteLine("\nDetenido");
        }

        Debug.WriteLine("\nTodos los servicios detenidos.");
        Thread.Sleep(1000);
    }
}
