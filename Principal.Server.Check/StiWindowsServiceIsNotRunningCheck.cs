using Principal.Server.Agent;

namespace Principal.Server.Check
{
    public class StiWindowsServiceIsNotRunningCheck : StiServerCheck
    {
        public override string ShortMessage => "El agente no está iniciado.";

        public override string LongMessage => "El agente no está iniciado. Este servicio proveé funcionalidad core a Principal Server.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsServiceNotRunned()
        {
            try
            {
                return StiServerAgentHelper.ServiceStatus != StiServiceStatus.Started;
            }
            catch
            {
                return true;
            }
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (IsServiceNotRunned())
            {
                return new StiWindowsServiceIsNotRunningCheck
                {
                    Element = obj,
                    Actions = { new StiStartServiceAction() }
                };
            }
            return null;
        }
    }
}
