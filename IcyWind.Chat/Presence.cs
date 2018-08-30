using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    public class Presence
    {
        public Jid FromJid { get; internal set; }
        public PresenceType PresenceType { get; internal set; }
        public PresenceShow PresenceShow { get; internal set; }
        public string Status { get; internal set; }
        public string LastOnline { get; set; }
    }
}
