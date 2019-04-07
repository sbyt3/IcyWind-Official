using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth
{
    internal class IcyWindHelpers
    {
        /// <summary>
        /// Converts any object into a base64 without the = sign at the end
        /// </summary>
        /// <param name="input">The object to convert</param>
        /// <returns>The resulting string</returns>
        internal static string ConvertToBase64Html(object input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input))).Replace("=", "");
        }

        /// <summary>
        /// Converts a string into base64 without the = sign at the end
        /// </summary>
        /// <param name="input">The string to convert</param>
        /// <returns>The resulting string</returns>
        internal static string ConvertToBase64Html(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).Replace("=", "");
        }


        /// <summary>
        /// Compute the hash of a given input
        /// </summary>
        /// <param name="input">The input to Hash</param>
        /// <returns>The input hashed</returns>
        internal static string Hash(string input)
        {
            using (var sha = new SHA256Managed())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
