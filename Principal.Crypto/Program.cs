using Principal.Server.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Principal.Crypto
{
    internal class Program
    {
        static void Main()
        {
            #region Uno
            //GenerateKeys();
            #endregion

            #region Dos
            var propNombre = new Reclamacion
            {
                Type = ClaimTypes.Name,
                Value = "Servicio Cancelación"
            };

            var propRol = new Reclamacion
            {
                Type = ClaimTypes.Role,
                Value = "Admin"
            };

            var identidad = new Identidad
            {
                Reclamaciones = new List<Reclamacion> { propNombre, propRol }
            };

            var privateKey = File.ReadAllText("private.key");
            NewSignData(identidad.Reclamaciones, privateKey);
            #endregion

            #region Tres
            var publickey = File.ReadAllText("public.key");
            var internalData = DeserializeData("cancelacion.sign");
            var inHeader = DeserializeDataAsString("cancelacion.sign");
            var isValid = VerifyData(inHeader, publickey);
            Console.WriteLine(isValid);
            #endregion
        }

        private static Identidad DeserializeData(string filePath)
        {
            var internalData = File.ReadAllBytes(filePath);
            using (var ms = new MemoryStream(internalData))
            {
                var bnfmt = new BinaryFormatter();
                return (Identidad)bnfmt.Deserialize(ms);
            }
        }

        private static string DeserializeDataAsString(string filePath)
        {
            var internalData = File.ReadAllBytes(filePath);
            string byteString = Convert.ToBase64String(internalData);

            return byteString;
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

        static void NewSignData(List<Reclamacion> reclamaciones, string privateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            List<Reclamacion> li = reclamaciones;
            var internalData = ToByteArray(li);
            var signature = rsa.SignData(internalData, new SHA1CryptoServiceProvider());

            var identidad = new Identidad
            {
                Reclamaciones = reclamaciones,
                Signature = signature
            };

            using (var ms = new MemoryStream())
            {
                var bnfmt = new BinaryFormatter();
                bnfmt.Serialize(ms, identidad);
                File.WriteAllBytes("cancelacion.sign", ms.GetBuffer());
            }
        }

        static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default;
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        static bool VerifyData(Identidad identidad, string publicKey)
        {
            if (identidad == null)
                throw new ArgumentNullException(nameof(identidad));

            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException(nameof(publicKey));

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            List<Reclamacion> li = identidad.Reclamaciones;
            var internalData = ToByteArray(li);

            return rsa.VerifyData(internalData, new SHA1CryptoServiceProvider(), identidad.Signature);
        }

        static bool VerifyData(string firma, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(firma))
                throw new ArgumentNullException(nameof(firma));

            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException(nameof(publicKey));

            byte[] mbytes = Convert.FromBase64String(firma);
            Identidad identidad = FromByteArray<Identidad>(mbytes);

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            List<Reclamacion> li = identidad.Reclamaciones;
            var internalData = ToByteArray(li);

            return rsa.VerifyData(internalData, new SHA1CryptoServiceProvider(), identidad.Signature);
        }
    }
}
