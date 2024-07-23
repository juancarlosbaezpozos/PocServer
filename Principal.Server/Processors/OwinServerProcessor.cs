using Microsoft.Extensions.Configuration;
using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using Principal.Server.Objects;
using System;

namespace Principal.Server.Processors
{
    public class OwinServerProcessor : StiProcessor
    {
        private IDisposable _server;
        private readonly string _baseUrl;

        public override string Name => "OwinServerProcessor";

        protected override TimeSpan SuspendTime => TimeSpan.FromSeconds(10);

        public OwinServerProcessor(IStiCore core, int? processorIndex, IConfiguration configuration) : base(core, processorIndex)
        {
            _baseUrl = configuration["OwinServer:Url"] ?? "http://localhost:8080";
        }

        protected override void Process()
        {
            if (_server == null && !string.IsNullOrWhiteSpace(_baseUrl))
            {
                _server = WebApp.Start<OwinServerStartup>(_baseUrl);
                System.Diagnostics.Debug.WriteLine("\nCargando tipo: " + typeof(OwinHttpListener) + "...");
                System.Diagnostics.Debug.WriteLine($"\n{Name} iniciando en {_baseUrl}");
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (_server != null)
            {
                _server.Dispose();
                _server = null;

                System.Diagnostics.Debug.WriteLine($"\n{Name} detenido.");
            }
        }
    }
}
