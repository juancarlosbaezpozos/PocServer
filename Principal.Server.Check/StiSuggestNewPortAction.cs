using System;
using System.Net.NetworkInformation;

namespace Principal.Server.Check
{
    public class StiSuggestNewPortAction : StiServerCheckerAction
    {
        public override string Name => "Buscar";

        public override string Description => "Buscar un puerto libre para el servidor.";

        public int Port { get; set; }

        public int NewPort { get; private set; }

        public StiSuggestNewPortAction(int port)
        {
            Port = port;
        }

        public override void Invoke(object element, string elementName)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (var tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port != Port)
                {
                    System.Diagnostics.Debug.WriteLine($"Puerto {tcpi.LocalEndPoint.Port} está disponible.");
                    NewPort = tcpi.LocalEndPoint.Port;
                    break;
                }
            }

            base.Invoke(element, elementName);
        }
    }
}
