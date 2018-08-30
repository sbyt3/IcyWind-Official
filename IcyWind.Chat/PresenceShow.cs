using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    public enum PresenceShow
    {
        Chat,
        Away,
        Dnd,
        Xa,
        Mobile
    }

    public class ConvertPresenceShow
    {
        public static string ConvertPresenceShowToString(PresenceShow show)
        {
            switch (show)
            {
                case PresenceShow.Away:
                    return "away";
                case PresenceShow.Chat:
                    return "chat";
                case PresenceShow.Mobile:
                    return "mobile";
                case PresenceShow.Dnd:
                    return "dnd";
                case PresenceShow.Xa:
                    return "xa";
                default:
                    return "chat";
            }
        }
    }
}
