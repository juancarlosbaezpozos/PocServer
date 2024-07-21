using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Principal.Server.Agent;

[RunInstaller(true)]
public class StiServerAgentInstaller : Installer
{
    public StiServerAgentInstaller()
    {
        ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
        ServiceInstaller serviceInstaller = new ServiceInstaller();
        serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
        serviceProcessInstaller.Username = null;
        serviceProcessInstaller.Password = null;
        serviceInstaller.DisplayName = StiServerAgent.DisplayAgentName;
        serviceInstaller.StartType = ServiceStartMode.Automatic;
        serviceInstaller.Description = StiServerAgent.ServerAgentDescription;
        serviceInstaller.ServiceName = StiServerAgent.ServerAgentName;
        base.Installers.Add(serviceProcessInstaller);
        base.Installers.Add(serviceInstaller);
    }
}
