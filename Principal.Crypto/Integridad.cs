using System;

namespace Principal.Crypto
{
    [Serializable]
    public class Integridad
    {
        public RegistroServicio Registro { get; set; }
        public byte[] Signature { get; set; }
    }
}
