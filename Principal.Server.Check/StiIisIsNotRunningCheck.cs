using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace Principal.Server.Check
{
    public class StiIisIsNotRunningCheck : StiServerCheck
    {
        public override string ShortMessage => "Internet Information Services no está iniciado.";

        public override string LongMessage => "Internet Information Services no está iniciado en este sistema.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsServiceNotStarted()
        {
            try
            {
                var processesByName = Process.GetProcessesByName("inetinfo");
                var processesByName2 = Process.GetProcessesByName("iisexpress");
                var processesByName3 = Process.GetProcessesByName("w3wp");
                var processesByName4 = Process.GetProcessesByName("w3svc");
                if (processesByName.Any() || processesByName2.Any() || processesByName3.Any() || processesByName4.Any())
                {
                    return false;
                }
                return new ServiceController("W3SVC").Status != ServiceControllerStatus.Running;
            }
            catch
            {
                return true;
            }
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (IsServiceNotStarted())
            {
                return new StiIisIsNotRunningCheck
                {
                    Element = obj
                };
            }
            return null;
        }
    }
}
