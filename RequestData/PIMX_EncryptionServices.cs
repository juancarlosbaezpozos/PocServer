using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RequestData
{
    public class PIMX_EncryptionServices
    {
        protected byte[] bytIV;
        protected string SecretPhrase;

        public PIMX_EncryptionServices() => this.bytIV = new byte[16]
        {
      (byte) 121,
      (byte) 241,
      (byte) 10,
      (byte) 1,
      (byte) 132,
      (byte) 74,
      (byte) 11,
      (byte) 39,
      byte.MaxValue,
      (byte) 91,
      (byte) 45,
      (byte) 78,
      (byte) 14,
      (byte) 211,
      (byte) 22,
      (byte) 62
        };

        private string GenerateSecretPhrase()
        {
            char[] chArray = new char[33];
            Random random = new Random();
            int index = 1;
            do
            {
                chArray[index] = Strings.Chr(random.Next() % 200);
                checked { ++index; }
            }
            while (index <= 32);
            return new string(chArray);
        }

        public string GetIV() => Convert.ToBase64String(this.bytIV);

        public string GenerateKey()
        {
            byte[] bytes = Encoding.ASCII.GetBytes(this.GenerateSecretPhrase());
            SHA384Managed shA384Managed = new SHA384Managed();
            shA384Managed.ComputeHash(bytes);
            return Convert.ToBase64String(shA384Managed.Hash);
        }

        public string DecryptString128Bit(string vstrStringToBeDecrypted, string vstrDecryptionKey)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            string empty = string.Empty;
            byte[] buffer = Convert.FromBase64String(vstrStringToBeDecrypted);
            if (Strings.Len(vstrDecryptionKey) >= 32)
            {
                vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32);
            }
            else
            {
                int Number = checked(32 - Strings.Len(vstrDecryptionKey));
                vstrDecryptionKey += Strings.StrDup(Number, "X");
            }
            byte[] bytes = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray());
            byte[] numArray = new byte[checked(buffer.Length + 1)];
            MemoryStream memoryStream = new MemoryStream(buffer);
            try
            {
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, rijndaelManaged.CreateDecryptor(bytes, this.bytIV), CryptoStreamMode.Read);
                cryptoStream.Read(numArray, 0, numArray.Length);
                cryptoStream.FlushFinalBlock();
                memoryStream.Close();
                cryptoStream.Close();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            return Encoding.ASCII.GetString(numArray);
        }

        public string EncryptString128Bit(string vstrTextToBeEncrypted, string vstrEncryptionKey)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytes1 = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted.ToCharArray());
            if (Strings.Len(vstrEncryptionKey) >= 32)
            {
                vstrEncryptionKey = Strings.Left(vstrEncryptionKey, 32);
            }
            else
            {
                int Number = checked(32 - Strings.Len(vstrEncryptionKey));
                vstrEncryptionKey += Strings.StrDup(Number, "X");
            }
            byte[] bytes2 = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray());
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] array = null;
            try
            {
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, rijndaelManaged.CreateEncryptor(bytes2, this.bytIV), CryptoStreamMode.Write);
                cryptoStream.Write(bytes1, 0, bytes1.Length);
                cryptoStream.FlushFinalBlock();
                array = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            return Convert.ToBase64String(array);
        }
    }
}
