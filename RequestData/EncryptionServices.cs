using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RequestData
{
    public class EncryptionServices
    {
        protected byte[] bytIV = new byte[16]
        {
            121, 241, 10, 1, 132, 74, 11, 39, byte.MaxValue, 91, 45, 78, 14, 211, 22, 62
        };
        protected string SecretPhrase;

        private static string GenerateSecretPhrase()
        {
            var chArray = new char[33];
            var random = new Random();
            for (var index = 0; index < 32; index++)
            {
                chArray[index] = (char)(random.Next() % 200);
            }
            return new string(chArray);
        }

        public string GetIV() => Convert.ToBase64String(this.bytIV);

        public string GenerateKey()
        {
            var bytes = Encoding.ASCII.GetBytes(GenerateSecretPhrase());
            using (var sha384 = SHA384.Create())
            {
                var hash = sha384.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public string DecryptString128Bit(string vstrStringToBeDecrypted, string vstrDecryptionKey)
        {
            var buffer = Convert.FromBase64String(vstrStringToBeDecrypted);
            vstrDecryptionKey = AdjustKeyLength(vstrDecryptionKey);

            var bytes = Encoding.ASCII.GetBytes(vstrDecryptionKey);
            var numArray = new byte[buffer.Length];

            using (var memoryStream = new MemoryStream(buffer))
            using (var rijndaelManaged = new RijndaelManaged())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(bytes, this.bytIV), CryptoStreamMode.Read))
            {
                try
                {
                    cryptoStream.Read(numArray, 0, numArray.Length);
                    cryptoStream.FlushFinalBlock();
                }
                catch (Exception)
                {
                    // Handle decryption error
                }
            }

            return Encoding.ASCII.GetString(numArray).TrimEnd('\0');
        }

        public string EncryptString128Bit(string vstrTextToBeEncrypted, string vstrEncryptionKey)
        {
            var bytes1 = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted);
            vstrEncryptionKey = AdjustKeyLength(vstrEncryptionKey);

            var bytes2 = Encoding.ASCII.GetBytes(vstrEncryptionKey);
            byte[] array;

            using (var memoryStream = new MemoryStream())
            using (var rijndaelManaged = new RijndaelManaged())
            using (var cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(bytes2, this.bytIV), CryptoStreamMode.Write))
            {
                try
                {
                    cryptoStream.Write(bytes1, 0, bytes1.Length);
                    cryptoStream.FlushFinalBlock();
                    array = memoryStream.ToArray();
                }
                catch (Exception)
                {
                    array = Array.Empty<byte>();
                }
            }

            return Convert.ToBase64String(array);
        }

        private static string AdjustKeyLength(string key)
        {
            return key.Length >= 32 ? key.Substring(0, 32) : key.PadRight(32, 'X');
        }
    }
}
