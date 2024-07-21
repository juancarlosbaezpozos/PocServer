using Principal.Server.Agent;

namespace Principal.Server.Check
{
    public class StiWindowsServiceIsNotInstalledCheck : StiServerCheck
    {
        public override string ShortMessage => "El agente no está instalado.";

        public override string LongMessage => "El agente no está instalado. Este servicio proveé funcionalidad core a Principal Server.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsServiceNotInstalled()
        {
            try
            {
                return StiServerAgentHelper.ServiceStatus == StiServiceStatus.NotInstalled;
            }
            catch
            {
                return true;
            }
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (IsServiceNotInstalled())
            {
                return new StiWindowsServiceIsNotInstalledCheck
                {
                    Element = obj,
                    Actions = { new StiInstallServiceAction() }
                };
            }
            return null;
        }
    }
}
