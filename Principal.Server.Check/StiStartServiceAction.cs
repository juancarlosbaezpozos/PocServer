using Principal.Server.Agent;

namespace Principal.Server.Check
{
    public class StiStartServiceAction : StiServerCheckerAction
    {
        public override string Name => "Iniciar";

        public override string Description => "Iniciar Agente.";

        public override void Invoke(object element, string elementName)
        {
            base.Invoke(element, elementName);
            StiServerController.StartServiceAsync().Wait();
        }
    }
}
