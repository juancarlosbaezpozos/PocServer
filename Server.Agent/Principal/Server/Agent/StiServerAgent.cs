using System;
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
        base.OnStart(args);
        Core = new StiCore();
        Core.Start();
    }

    protected override void OnStop()
    {
        base.OnStop();
        Core.Stop();
        Core = null;
        base.ExitCode = 0;
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
            ServiceBase.Run(servicesToRun);
        }
    }

    public StiServerAgent()
    {
        base.ServiceName = ServerAgentName;
        EventLog.Log = "Application";
        base.CanHandlePowerEvent = true;
        base.CanHandleSessionChangeEvent = true;
        base.CanPauseAndContinue = true;
        base.CanShutdown = true;
        base.CanStop = true;
    }

    private static void RunInteractive(ServiceBase[] servicesToRun)
    {
        Console.WriteLine("Servicios corriendo en modo interactivo.");
        Console.WriteLine();

        var onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
            BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var service in servicesToRun)
        {
            Console.Write("Iniciando {0}...", service.ServiceName);
            onStartMethod?.Invoke(service, new object[] { new string[] { } });
            Console.Write("Iniciado");
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(
            "Presione cualquier tecla para detener el servicio y terminar el proceso...");
        Console.ReadKey();
        Console.WriteLine();

        var onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
            BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var service in servicesToRun)
        {
            Console.Write("Deteniendo {0}...", service.ServiceName);
            onStopMethod?.Invoke(service, null);
            Console.WriteLine("Detenido");
        }

        Console.WriteLine("Todos los servicios detenidos.");
        Thread.Sleep(1000);
    }
}
