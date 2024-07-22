using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Principal.Crypto
{
    internal class Program
    {
        static void Main()
        {
            #region Uno
            GenerateKeys();
            #endregion

            #region Dos
            var registro = new RegistroServicio();
            registro.Nombre = "Servicio de Facturación";
            registro.Pin = "1234";

            var privateKey = File.ReadAllText("private.key");
            SignData(registro, privateKey);
            #endregion

            #region Tres
            var publickey = File.ReadAllText("public.key");
            var internalData = DeserializeData("registro.sign");
            var isValid = VerifyData(internalData, publickey);
            Console.WriteLine(isValid);
            #endregion
        }

        private static Integridad DeserializeData(string filePath)
        {
            var internalData = File.ReadAllBytes(filePath);
            using (var ms = new MemoryStream(internalData))
            {
                var bnfmt = new BinaryFormatter();
                return (Integridad)bnfmt.Deserialize(ms);
            }
        }

        static void GenerateKeys()
        {
            var rsa = new RSACryptoServiceProvider();
            var privateKey = rsa.ToXmlString(true);
            var publicKey = rsa.ToXmlString(false);

            var privateKeyPath = "private.key";
            var publicKeyPath = "public.key";

            File.WriteAllText(privateKeyPath, privateKey);
            File.WriteAllText(publicKeyPath, publicKey);
        }

        static void SignData(RegistroServicio servicio, string privateKey)
        {
            if (servicio == null)
                throw new ArgumentNullException(nameof(servicio));

            if (string.IsNullOrEmpty(privateKey))
                throw new ArgumentNullException(nameof(privateKey));

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            var internalData = servicio.GetRegistroData();
            var signature = rsa.SignData(internalData, new SHA1CryptoServiceProvider());

            var integrity = new Integridad();
            integrity.Registro = servicio;
            integrity.Signature = signature;

            using (var ms = new MemoryStream())
            {
                var bnfmt = new BinaryFormatter();
                bnfmt.Serialize(ms, integrity);
                File.WriteAllBytes("registro.sign", ms.GetBuffer());
            }
        }

        static bool VerifyData(Integridad integrity, string publicKey)
        {
            if (integrity == null)
                throw new ArgumentNullException(nameof(integrity));

            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException(nameof(publicKey));

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            var internalData = integrity.Registro.GetRegistroData();

            return rsa.VerifyData(internalData, new SHA1CryptoServiceProvider(), integrity.Signature);
        }
    }
}
