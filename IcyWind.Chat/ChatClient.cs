﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using IcyWind.Chat.Auth;
using IcyWind.Chat.Jid;
using IcyWind.Chat.Presence;
using IcyWind.Chat.TcpConnection;

namespace IcyWind.Chat
{
    public class ChatClient
    {
        #region XMPPClientData
        internal AuthHandler AuthHandler { get; set; }
        internal TcpStringClient Client { get; set; }

        public PresenceManager PresenceManager { get; internal set; }
        #endregion XMPPClientData

        #region InternalVars
        internal int Id { get; set; }
        internal bool Disconnecting { get; set; }
        internal bool HasHandledAuth { get; set; } = false;
        internal AuthCred AuthCred { get; private set; }
        public BaseAuth AuthMethod { get; private set; }
        #endregion InternalVars
        
        #region Delegates
        /// <summary>
        /// Used to tell that the Auth string was sent. Sends the AuthToken
        /// </summary>
        /// <param name="rsoToken"></param>
        public delegate void HandledAuth(string rsoToken);
        public event HandledAuth HandledRsoAuth;

        /// <summary>
        /// Returned when the server returns a successful login
        /// </summary>
        public delegate void SuccessLogin();
        public event SuccessLogin OnSuccessLogin;

        /// <summary>
        /// Called when the presence is being requested for XMPP
        /// </summary>
        /// <returns>Presence</returns>
        public delegate string GetPresence();
        public event GetPresence GetChatPresence;

        /// <summary>
        /// Called when a RostItem has been recieved
        /// </summary>
        /// <param name="jid">The user JID</param>
        public delegate void RostItem(UserJid jid);
        public event RostItem OnRosterItemRecieved;

        /// <summary>
        /// Got when a presence has been recieved.
        /// </summary>
        /// <param name="pres">The presence</param>
        public delegate void OnPresence(ChatPresence pres);
        public event OnPresence OnPlayerPresenceReceived;
        public event OnPresence OnMobilePresenceReceived;

        /// <summary>
        /// Called when a message is recieved
        /// </summary>
        /// <param name="from">The user's JID</param>
        /// <param name="message">The message from that user</param>
        public delegate void OnMessage(UserJid from, string message);
        public event OnMessage OnMessageReceived;
        #endregion Delegates

        #region PublicVars

        /// <summary>
        /// The current JID (Your JID)
        /// </summary>
        public UserJid MainJid { get; internal set; }

        #endregion PublicVars

        public ChatClient(IPEndPoint endPoint)
        {
            Client = new TcpStringClient(endPoint, true);
        }

        public ChatClient(IPEndPoint endPoint, bool ssl)
        {
            Client = new TcpStringClient(endPoint, ssl);
        }

        public async Task ConnectSSL(string host, AuthCred cred)
        {
            PresenceManager = new PresenceManager(this);
            await Client.ConnectSSL(host);
            Client.SendString(
                $"<stream:stream to=\"{host}\" xml:lang=\"*\" version=\"1.0\" xmlns:stream=\"http://etherx.jabber.org/streams\" xmlns=\"jabber:client\">");
            AuthCred = cred;
        }

        public bool AddSASLAuth(BaseAuth authMethod)
        {
            AuthMethod = authMethod;
            return true;
        }

        public ChatRoom JoinRoom(UserJid roomJid, string password)
        {
            if (roomJid.Type != JidType.GroupChatJid)
            {
                throw new InvalidJidTypeException("To join a room, the Jid must be a type of GroupChat");
            }

            var roomJoinString = Encoding.UTF8.GetBytes(
                $"<presence from=\'{MainJid.RawJid}\' to=\'{roomJid.RawJid}\'>" +
                "<x xmlns=\'http://jabber.org/protocol/muc\'>" +
                $"<password>{password}</password>" +
                "</x>" +
                "</presence>");

            return new ChatRoom(this, roomJid);
        }

        public ChatRoom JoinRoom(UserJid roomJid)
        {
            if (roomJid.Type != JidType.GroupChatJid)
            {
                throw new InvalidJidTypeException("To join a room, the Jid must be a type of GroupChat");
            }

            Client.SendString($"<presence from=\'{MainJid.RawJid}\' to=\'{roomJid.RawJid}\'>" +
                              "<x xmlns=\'http://jabber.org/protocol/muc\'/>" +
                              "</presence>");
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            return new ChatRoom(this, roomJid);
        }

        /// <summary>
        /// Send a message to a user
        /// </summary>
        /// <param name="to">The user's Jid you want to send the message to</param>
        /// <param name="message">The message to send</param>
        public void SendMessage(UserJid to, string message)
        {
            //Set to high priority
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var encodedXml = System.Web.HttpUtility.HtmlEncode(message);

            Client.SendString($"<message from=\'{MainJid.RawJid}\' to=\'{to.RawJid}\' type=\'chat\'><body>{encodedXml}</body></message>");
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        /// <summary>
        /// Sends the disconnect packet
        /// </summary>
        public void Disconnect()
        {
            Client.SendString("</stream:stream>");
            Disconnecting = true;
        }

        /// <summary>
        /// The handler for when the string is recieved
        /// </summary>
        /// <param name="x">The recieved string</param>
        private bool ReadXMPPMessage(string x)
        {
            if (x.Contains("</stream:stream>"))
            {
                //TODO: Handle a disconnect
            }
            //Lazy hack, this converts the initial starting output to a valid XML doc
            else if (x.Contains("<stream:stream"))
            {
                x += "</stream:stream>";
            }
            //Lazy hack 2, This stops any stupid blah blah blah is not a part of this domain stuff
            x = x.Replace("stream:", "");


            try
            {
                //The XML Reader
                using (var reader = XmlReader.Create(new StringReader(x)))
                {
                    //Convert to XmlDocument for reading
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    var el = doc.DocumentElement;

                    #region XmppPresenceHandler

                    if (el.Name == "presence" && el.HasChildNodes)
                    {
                        PresenceManager.HandleReceivedPresence(el);
                    }

                    #endregion XmppPresenceHandler

                    #region XmppMessageRecieveHandler

                    else if (el.Name == "message" && el.HasChildNodes)
                    {
                        try
                        {
                            if (el.Attributes["from"].Value == el.Attributes["to"].Value)
                                return true;

                            //TODO: Add a message handler
                            //OnMessageRecieved?.Invoke(new Jid(el.Attributes["from"].Value), el.InnerText);
                        }
                        catch
                        {
                            //Ignore for now
                        }
                    }

                    #endregion XmppMessageRecieveHandler

                    else if (el.HasChildNodes)
                    {
                        foreach (var node in el.ChildNodes)
                        {
                            var xmlNode = (XmlNode) node;

                            //Handle RsoAuth

                            #region AuthHandler

                            if (xmlNode.Name == "mechanisms")
                            {
                                if (AuthHandler.HandleAuth(xmlNode))
                                {
                                    break;
                                }
                            }

                            #endregion AuthHandler

                            #region ConnectionSuccessHandler

                            if (el.Name == "success" && xmlNode.Name == "#text")
                            {
                                //I have no idea why I did this, but I did it. Don't question me Wild
                                if (int.TryParse(xmlNode.InnerText, out var output))
                                    Id = output;

                                //Say we logged in okay
                                OnSuccessLogin?.Invoke();

                                //Send another random static string
                                Client.SendString(
                                    "<stream:stream " +
                                    "to=\"pvp.net\" " +
                                    "xml:lang=\"*\" " +
                                    "version=\"1.0\" " +
                                    "xmlns:stream=\"http://etherx.jabber.org/streams\" " +
                                    "xmlns=\"jabber:client\">");
                            }

                            #endregion ConnectionSuccessHandler

                            #region IDontKnowWtfThisIsButReturnBinding

                            if (xmlNode.Name == "rxep")
                            {
                                //This is sent from client to server after this is received. I honestly don't know this xmpp stuff so lets pretend we read that

                                Client.SendString("<iq type=\"set\" id=\"0\"><bind xmlns=\"urn:ietf:params:xml:ns:xmpp-bind\"><resource>RC</resource></bind></iq>");

                                break;
                            }

                            #endregion IDontKnowWtfThisIsButReturnBinding

                            #region HandleSessionIQ

                            if (el.Name == "iq" && xmlNode.Name == "bind" && xmlNode.HasChildNodes)
                            {
                                //Make sure that this is the correct IQ and stuff
                                if (xmlNode.ChildNodes.Count == 1 &&
                                    xmlNode.FirstChild.Name == "jid")
                                {
                                    MainJid = new UserJid(xmlNode.InnerText);

                                    Client.SendString("<iq type=\"set\" id=\"1\"><session xmlns=\"urn:ietf:params:xml:ns:xmpp-session\"/></iq>");
                                }
                            }

                            #endregion HandleSessionIQ

                            #region SessionIQHandler

                            if (el.Name == "iq" && xmlNode.Name == "session")
                            {
                                //Make sure that this is the correct thing
                                if (xmlNode.ChildNodes.Count == 2 &&
                                    xmlNode.LastChild.Name == "summoner_name")
                                {
                                    MainJid.SumName = xmlNode.LastChild.InnerText;
                                    //Require presence from the client. The client should use PresenceAsString to craft this presence
                                    var presence = GetChatPresence?.Invoke();
                                    if (!string.IsNullOrEmpty(presence))
                                    {
                                        //Write the presence to all clients
                                        Client.SendString(presence);
                                    }

                                    //Request roster and priv_req (I think this is friend requests)
                                    Client.SendString(
                                        $"<iq type=\"get\" id=\"priv_req_2\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:privacy\"><list name=\"LOL\"/></query></iq>");
                                    Client.SendString(
                                        $"<iq type=\"get\" id=\"rst_req_3\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:roster\"/></iq>");
                                    var date = "";

                                    //Lazy hack to subtract a month from today's date
                                    date = DateTime.Now.Month == 1
                                        ? $"{DateTime.Now.Year - 1}-12-{DateTime.Now.Day}"
                                        : $"{DateTime.Now.Year}-{DateTime.Now.Month - 1}-{DateTime.Now.Day}";

                                    //Retrieve any former messages from the history of the archives
                                    Client.SendString(
                                        $"<iq type=\"get\" id=\"recent_conv_req_4\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:archive:list\">" +
                                        $"<since>{date} 00:00:00</since><count>10</count></query></iq>");
                                }
                            }

                            #endregion SessionIQHandler

                            #region RosterIQ

                            if (el.Name == "iq" && el.HasAttribute("id") &&
                                     el.Attributes["id"].InnerText == "rst_req_3")
                            {
                                if (xmlNode.HasChildNodes)
                                {
                                    foreach (var itemNode in xmlNode.ChildNodes)
                                    {
                                        var itemRostNode = (XmlNode) itemNode;

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
                        }
                    }
                    else
                    {
                        //TODO: <?xml version='1.0'?><stream:stream xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' id='3891861776' from='pvp.net' version='1.0'>
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
