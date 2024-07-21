using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using Principal.Server.Objects;
using System;

namespace Principal.Server.Processors
{
    public class OwinServerProcessor : StiProcessor
    {
        private IDisposable _server;

        public override string Name => "OwinServerProcessor";

        protected override TimeSpan SuspendTime => TimeSpan.FromSeconds(1);

        public OwinServerProcessor(IStiCore core, int? processorIndex) : base(core, processorIndex)
        {
        }

        protected override void Process()
        {
            if (_server == null)
            {
                _server = WebApp.Start<OwinServerStartup>("http://localhost:8080");
                Console.WriteLine("\nCargando tipo: " + typeof(OwinHttpListener) + "...");
                Console.WriteLine($"\n{Name} iniciando en http://localhost:8080");
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
                Console.WriteLine($"\n{Name} detenido.");
            }
        }
    }
}
