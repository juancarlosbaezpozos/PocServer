using Principal.Server.Agent;
using System.Threading.Tasks;

namespace Principal.Server.Check
{
    public class StiInstallServiceAction : StiServerCheckerAction
    {
        public override string Name => "Instalar";

        public override string Description => "Instalar Agente.";

        public override bool RemoveCheckAfterInvokeAction => true;

        public override async void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            await InstallServiceAsync();
        }

        private async Task InstallServiceAsync()
        {
            await StiServerController.InstallServiceAsync();
        }
    }
}
