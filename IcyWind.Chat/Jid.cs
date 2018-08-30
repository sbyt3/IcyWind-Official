using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    public class Jid
    {
        public string RawJid { get; }

        public string PlayerJid { get; }

        public string SumName { get; internal set; }

        public string Extra { get; internal set; }

        public string Group { get; internal set; }

        public Jid(string rJid)
        {
            RawJid = rJid;
            if (rJid.Contains("/"))
            {
                var data = rJid.Split('/');
                if (data.Length >= 2)
                {
                    Extra = data[1];
                }

                PlayerJid = data.First();
            }
            else
            {
                PlayerJid = rJid;
            }
        }
    }
}
