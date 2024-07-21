using Principal.Server.Agent;

namespace Principal.Server.Check
{
    public class StiInstallServiceAction : StiServerCheckerAction
    {
        public override string Name => "Instalar";

        public override string Description => "Instalar Agente.";

        public override void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            StiServerController.InstallServiceAsync().Wait();
        }
    }
}
