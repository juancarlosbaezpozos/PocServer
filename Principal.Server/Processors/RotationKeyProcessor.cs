using System;
using Principal.Server.Objects;

namespace Principal.Server.Processors
{
    public class RotationKeyProcessor : StiProcessor
    {
        protected override TimeSpan SuspendTime { get; } = TimeSpan.FromDays(1);

        public override string Name => "RotationKeyProcessor";

        public RotationKeyProcessor(IStiCore core, int? processorIndex)
            : base(core, processorIndex)
        {
        }

        protected override void Process()
        {
            try
            {
                ProcesarRotacion();
            }
            catch
            {
            }
        }

        protected void ProcesarRotacion()
        {
            //TODO: procesar rotación llaves
        }
    }
}
