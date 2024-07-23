﻿using Principal.Server.Objects;
using Principal.Server.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web.Http;

namespace Principal.Server.Guards
{
    internal static class RequestValidator
    {
        public static void ValidateAppRole(string appRole)
        {
            Claim roleClaim = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || !roleClaim.Value.Split(' ').Contains(appRole))
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    ReasonPhrase = $"Los 'roles' reclamados no contienen '{appRole}' o no se encontraron."
                });
            }
        }

        public static bool ValidSign(string firma, out List<Reclamacion> reclamaciones)
        {
            if (string.IsNullOrWhiteSpace(firma))
                throw new ArgumentNullException(nameof(firma));

            byte[] mbytes = Convert.FromBase64String(firma);
            Identidad identidad = FromByteArray<Identidad>(mbytes);

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(Resources.public_key);

            reclamaciones = identidad.Reclamaciones;
            var internalData = ToByteArray(reclamaciones);

            return rsa.VerifyData(internalData, new SHA1CryptoServiceProvider(), identidad.Signature);
        }

        static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream(data))
                {
                    object obj = bf.Deserialize(ms);
                    return (T)obj;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}