using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IcyWind.Chat.Jid;

namespace IcyWind.Chat.Iq
{
    /// <summary>
    /// This 
    /// </summary>
    public class IqHandler
    {
        internal ChatClient ChatClient { get; set; }

        public IqHandler(ChatClient client)
        {
            ChatClient = client;
        }

        public bool HandleIq(XmlNode xmlNode, XmlElement el)
        {
            #region HandleSessionIQ

            if (xmlNode.Name == "bind" && xmlNode.HasChildNodes)
            {
                //Make sure that this is the correct IQ and stuff
                if (xmlNode.ChildNodes.Count == 1 &&
                    xmlNode.FirstChild.Name == "jid")
                {
                    ChatClient.MainJid = new UserJid(xmlNode.InnerText)
                    {
                        Type = JidType.FriendChatJid,
                    };

                    ChatClient.TcpClient.SendString("<iq type=\"set\" id=\"1\"><session xmlns=\"urn:ietf:params:xml:ns:xmpp-session\"/></iq>");
                }
            }

            #endregion HandleSessionIQ

            #region SessionIQHandler

            if (xmlNode.Name == "session")
            {
                //Make sure that this is the correct thing
                if (xmlNode.ChildNodes.Count == 2 &&
                    xmlNode.LastChild.Name == "summoner_name")
                {
                    ChatClient.MainJid.SumName = xmlNode.LastChild.InnerText;

                    if (!string.IsNullOrEmpty(presence))
                    {
                        //Write the presence to all clients
                        ChatClient.Client.SendString(presence);
                    }

                    //Request roster and priv_req (I think this is friend requests)
                    ChatClient.TcpClient.SendString(
                        $"<iq type=\"get\" id=\"priv_req_2\" to=\"{ChatClient.MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:privacy\"><list name=\"LOL\"/></query></iq>");
                    ChatClient.TcpClient.SendString(
                        $"<iq type=\"get\" id=\"rst_req_3\" to=\"{ChatClient.MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:roster\"/></iq>");

                    //Lazy hack to subtract a month from today's date
                    var date = DateTime.Now.Month == 1
                        ? $"{DateTime.Now.Year - 1}-12-{DateTime.Now.Day}"
                        : $"{DateTime.Now.Year}-{DateTime.Now.Month - 1}-{DateTime.Now.Day}";

                    //Retrieve any former messages from the history of the archives
                    ChatClient.TcpClient.SendString(
                        $"<iq type=\"get\" id=\"recent_conv_req_4\" to=\"{ChatClient.MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:archive:list\">" +
                        $"<since>{date} 00:00:00</since><count>10</count></query></iq>");
                }
            }

            #endregion SessionIQHandler

            #region RosterIQ

            if (el.HasAttribute("id") &&
                     el.Attributes["id"].InnerText == "rst_req_3")
            {
                if (xmlNode.HasChildNodes)
                {
                    foreach (var itemNode in xmlNode.ChildNodes)
                    {
                        var itemRostNode = (XmlNode)itemNode;

                        try
                        {
                            //Create the JID
                            var inJid = new UserJid(itemRostNode.Attributes["jid"].Value)
                            {
                                SumName = itemRostNode.Attributes["name"].Value,
                                Group = itemRostNode.HasChildNodes
                                    ? itemRostNode.FirstChild.InnerText
                                    : "**Default",
                                Type = JidType.FriendChatJid,
                            };

                            //If the user has a group print it, otherwise return **Default

                            //Send the jid
                            OnRosterItemRecieved?.Invoke(inJid);
                        }
                        catch
                        {
                            // Don't know if we find attr
                        }
                    }
                }
            }

            #endregion RosterIQ

            return true;
        }
    }
}
