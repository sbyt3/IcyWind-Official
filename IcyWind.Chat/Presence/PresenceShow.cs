namespace IcyWind.Chat.Presence
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
