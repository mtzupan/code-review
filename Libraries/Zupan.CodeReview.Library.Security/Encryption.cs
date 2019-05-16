using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Zupan.CodeReview.Library.Security
{
    public static class Encryption
    {
        private const string SalthPhrase = "OTyCN^Ix";
        private const string IvPhrase = "NobjoSVc";

        private static readonly byte[] Salt;
        private static readonly byte[] Iv;

        static Encryption()
        {
            Salt = Encoding.UTF8.GetBytes(SalthPhrase);
            Iv = Encoding.UTF8.GetBytes(IvPhrase);
        }

        public static string Encrypt(string text)
        {
            var des = new DESCryptoServiceProvider();
            var byteArray = Encoding.UTF8.GetBytes(text);

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                des.CreateEncryptor(Salt, Iv), CryptoStreamMode.Write);

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decrypt(string text)
        {
            try
            {
                var des = new DESCryptoServiceProvider();
                var byteArray = Convert.FromBase64String(text);

                var memoryStream = new MemoryStream();
                var cryptoStream = new CryptoStream(memoryStream,
                    des.CreateDecryptor(Salt, Iv), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
