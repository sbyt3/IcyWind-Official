﻿using System.Linq;

namespace IcyWind.Chat.Jid
{
    public class UserJid
    {
        public static bool operator== (UserJid orgJid, UserJid compJid)
        {
            return orgJid != null && (compJid != null && orgJid.PlayerJid == compJid.PlayerJid);
        }

        public static bool operator!= (UserJid orgJid, UserJid compJid)
        {
            return compJid != null && (orgJid != null && orgJid.PlayerJid != compJid.PlayerJid);
        }

        public override bool Equals(object obj)
        {
            if (obj is UserJid compJid)
            {
                if (this == compJid)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return RawJid;
        }

        public string RawJid { get; }

        public string PlayerJid { get; }

        public string SumName { get; internal set; }

        public string Extra { get; internal set; }

        public string Group { get; internal set; }

        public JidType Type { get; internal set; } = JidType.UnknownJid;

        internal UserJid(string rJid)
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

    public enum JidType
    {
        FriendChatJid,
        GroupChatJid,
        UnknownJid,
    }
}
