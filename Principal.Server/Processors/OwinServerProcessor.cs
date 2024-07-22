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
        private readonly IConfiguration _configuration;

        public override string Name => "OwinServerProcessor";

        protected override TimeSpan SuspendTime => TimeSpan.FromSeconds(1);

        public OwinServerProcessor(IStiCore core, int? processorIndex, IConfiguration configuration) : base(core, processorIndex)
        {
            _configuration = configuration;
            _baseUrl = _configuration["OwinServer:Url"];
        }

        protected override void Process()
        {
            if (_server == null)
            {
                _server = WebApp.Start<OwinServerStartup>(_baseUrl);
                Console.WriteLine("\nCargando tipo: " + typeof(OwinHttpListener) + "...");
                Console.WriteLine($"\n{Name} iniciando en {_baseUrl}");
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
