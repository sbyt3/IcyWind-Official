using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    public enum PresenceType
    {
        Available,
        Subscribe,
        Subscribed,
        Unsubscribe,
        Unsubscribed,
        Unavailable,
        Invisible,
        Visible,
        Error,
        Probe
    }

    public class ConvertPresenceType
    {
        public static string ConvertPresenceTypeToString(PresenceType type)
        {
            switch (type)
            {
                case PresenceType.Available:
                    return "available";
                case PresenceType.Error:
                    return "error";
                case PresenceType.Invisible:
                    return "invis";
                case PresenceType.Probe:
                    return "probe";
                case PresenceType.Subscribe:
                    return "subscribe";
                case PresenceType.Subscribed:
                    return "subscribed";
                case PresenceType.Unavailable:
                    return "unavailabe";
                case PresenceType.Unsubscribe:
                    return "unsubscribe";
                case PresenceType.Unsubscribed:
                    return "unsubscribed";
                case PresenceType.Visible:
                    return "visible";
                default:
                    return type.ToString("G").ToLower();
            }
        }
    }
}
