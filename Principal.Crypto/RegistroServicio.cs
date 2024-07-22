using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Principal.Crypto
{
    [Serializable]
    public class RegistroServicio
    {
        public string Nombre { get; set; }
        public string Pin { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(" Nombre: " + Nombre);
            builder.Append(" Pin: " + Pin);
            return builder.ToString();
        }

        public byte[] GetRegistroData()
        {
            using (var ms = new MemoryStream())
            {
                var bnfmt = new BinaryFormatter();
                bnfmt.Serialize(ms, this);
                return ms.GetBuffer();
            }
        }
    }
}
