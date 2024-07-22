using System.Net.NetworkInformation;

namespace Principal.Server.Check
{
    public class StiPortAvailabilityCheck : StiServerCheck
    {
        private const int TargetPort = 8080;

        public override string ElementName => "PortAvailability";

        public override string ShortMessage => "El puerto 8080 no está disponible.";

        public override string LongMessage => "El puerto 8080 está siendo utilizado por otro proceso. Se sugiere utilizar otro puerto.";

        public override StiCheckServerStatus ServerStatus => StiCheckServerStatus.Error;

        private bool IsPortAvailable(int port)
        {
            var isAvailable = true;

            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (var tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        public override object ProcessCheck(object obj)
        {
            base.Element = obj;
            if (!IsPortAvailable(TargetPort))
            {
                return new StiPortAvailabilityCheck
                {
                    Element = obj,
                    Actions = { new StiSuggestNewPortAction(TargetPort) }
                };
            }
            return null;
        }
    }
}
