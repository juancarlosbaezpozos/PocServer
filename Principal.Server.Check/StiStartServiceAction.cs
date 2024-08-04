using System.Threading.Tasks;
using Principal.Server.Agent;

namespace Principal.Server.Check
{
    public class StiStartServiceAction : StiServerCheckerAction
    {
        public override string Name => "Iniciar";

        public override string Description => "Iniciar Agente.";

        public override bool RemoveCheckAfterInvokeAction => true;

        public override async void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            await StartServiceAsync();
        }

        private async Task StartServiceAsync()
        {
            await StiServerController.StartServiceAsync();
        }
    }
}
