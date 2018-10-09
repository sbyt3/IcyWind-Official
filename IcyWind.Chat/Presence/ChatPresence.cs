namespace IcyWind.Chat.Presence
{
    public class ChatPresence
    {
        public Jid FromJid { get; internal set; }
        public PresenceType PresenceType { get; internal set; }
        public PresenceShow PresenceShow { get; internal set; }
        public string Status { get; internal set; }
        public string LastOnline { get; set; }
    }
}
