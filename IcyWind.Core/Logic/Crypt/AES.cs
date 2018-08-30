using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.Crypt
{
    public static class AES
    {

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string EncryptBase64(string key, string inputText)
        {
            var keyAndIvBytes = StringToByteArray(key);
            string sRet;
            var rj = new RijndaelManaged();
            try
            {
                rj.Key = keyAndIvBytes;
                rj.IV = keyAndIvBytes;
                var ms = new MemoryStream();

                using (var cs = new CryptoStream(ms, rj.CreateEncryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(inputText);
                    }
                }
                var encoded = ms.ToArray();
                sRet = Convert.ToBase64String(encoded);

            }
            finally
            {
                rj.Clear();
            }

            return sRet;
        }

        public static string DecryptBase64(string key, string inputText)
        {
            var keyAndIvBytes = StringToByteArray(key);
            string sRet;
            var rj = new RijndaelManaged();
            try
            {
                var message = Convert.FromBase64String(inputText);
                rj.Key = keyAndIvBytes;
                rj.IV = keyAndIvBytes;
                var ms = new MemoryStream(message);

                using (var cs = new CryptoStream(ms, rj.CreateDecryptor(keyAndIvBytes, keyAndIvBytes), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        sRet = sr.ReadToEnd();
                    }
                }

            }
            finally
            {
                rj.Clear();
            }

            return sRet;
        }
    }
}
