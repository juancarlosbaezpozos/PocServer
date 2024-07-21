using Principal.Server.Objects;
using System;

namespace Principal.Server
{
    public class EjemploProcessor : StiProcessor
    {
        protected override TimeSpan SuspendTime { get; } = TimeSpan.FromSeconds(20.0);

        public override string Name => "Ejemplo-Processor";

        public EjemploProcessor(IStiCore core, int? processorIndex)
            : base(core, processorIndex)
        {
        }

        protected override void Process()
        {
            try
            {
                ProcesarRotacion();
            }
            catch (Exception exception)
            {
            }
        }

        protected void ProcesarRotacion()
        {
            //TODO: procesar rotación llaves
        }
    }
}
