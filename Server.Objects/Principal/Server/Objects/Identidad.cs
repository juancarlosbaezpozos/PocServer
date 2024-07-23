using System;
using System.Collections.Generic;

namespace Principal.Server.Objects
{
    [Serializable]
    public class Identidad
    {
        public List<Reclamacion> Reclamaciones { get; set; }

        public byte[] Signature { get; set; }
    }
}
