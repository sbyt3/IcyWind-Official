using System;
using System.Runtime.InteropServices;
using System.Text;

namespace IcyWind.Chat.Auth.Sasl
{
    public class Plain : BaseAuth
    {
        public override string AuthMethod => "PLAIN";

        public Plain(ChatClient client) : base(client, client.AuthCred) { }

        public override void HandleAuth()
        {
            var sb = new StringBuilder();
            //Create the plain auth string
            sb.Append((char)0);
            sb.Append(AuthCred.Username);
            sb.Append((char)0);
            //Check if secure password is used instead of password and prefer it over password
            if (AuthCred.SecurePassword != null)
            {
                var ptr = IntPtr.Zero;
                try
                {
                    ptr = Marshal.SecureStringToBSTR(AuthCred.SecurePassword);
                    sb.Append(Marshal.PtrToStringUni(ptr));
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
                }
            }
            else
            {
#pragma warning disable 618
                sb.Append(AuthCred.Password);
#pragma warning restore 618
            }
            //Send the string to the server
            ChatClient.TcpClient.SendString($"<sasl:auth mechanism=\'{AuthMethod}\'>{sb}</sasl:auth>");
            sb.Clear();
            // ReSharper disable once RedundantAssignment
            sb = null;
        }
    }
}
