using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace IcyWind.Chat
{
    public class ChatClient
    {
        #region ServerData
        internal TcpClient Client { get; set; }
        internal SslStream SslStream { get; set; }
        internal IPEndPoint EndPoint { get; set; }
        #endregion ServerData

        #region InternalVars
        internal int Id { get; set; }
        internal string Server { get; set; } = "pvp.net";

        internal bool Disconnecting { get; set; }
        internal bool HasHandledAuth { get; set; } = false;
        internal string RsoToken { get; set; }
        #endregion InternalVars
        
        #region Delegates
        /// <summary>
        /// Used internally when a string is recieved from the server
        /// </summary>
        /// <param name="x"></param>
        internal delegate bool RecString(string x);
        internal event RecString RecieveString;

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
        public delegate void RostItem(Jid jid);
        public event RostItem OnRosterItemRecieved;

        /// <summary>
        /// Got when a presence has been recieved.
        /// </summary>
        /// <param name="pres">The presence</param>
        public delegate void OnPresence(Presence pres);
        public event OnPresence OnPlayerPresenceRecieved;
        public event OnPresence OnMobilePresenceRecieved;

        /// <summary>
        /// Called when a message is recieved
        /// </summary>
        /// <param name="from">The user's JID</param>
        /// <param name="message">The message from that user</param>
        public delegate void OnMessage(Jid from, string message);
        public event OnMessage OnMessageRecieved;
        #endregion Delegates

        #region PublicVars

        /// <summary>
        /// The current JID (Your JID)
        /// </summary>
        public Jid MainJid { get; internal set; }

        #endregion PublicVars

        public ChatClient(IPEndPoint endPoint)
        {
            //Save the IP Address
            EndPoint = endPoint;
            //Create the TcpClient for sending data to the server
            Client = new TcpClient {NoDelay = true};
            //Set the socket option
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
        }

        public ChatClient(IPEndPoint endPoint, string server)
        {
            //Same stuff
            Disconnecting = false;
            EndPoint = endPoint;
            Server = server;
            Client = new TcpClient { NoDelay = true };
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
        }

        public async Task Connect(string rso)
        {
            //Set the RSO Token
            RsoToken = rso;
            Disconnecting = false;
            //Connect to Server
            await Client.ConnectAsync(EndPoint.Address, EndPoint.Port);
            //Create the SslStream for talking to the rito gems server
            SslStream = new SslStream(Client.GetStream(), true, (sender, certificate, chain, errors) => true);

            //Start stream auth
            SslStream.AuthenticateAsClient("pvp.net", null, SslProtocols.Tls, false);

            //Handle when strings are recieved
            RecieveString += ChatServer_RecieveString;
            StartReadLoop();

            //Send the connection string and start the read loop
            var connection = Encoding.UTF8.GetBytes(
                $"<stream:stream to=\"{Server}\" xml:lang=\"*\" version=\"1.0\" xmlns:stream=\"http://etherx.jabber.org/streams\" xmlns=\"jabber:client\">");
            SslStream.Write(connection);
        }
        /// <summary>
        /// Lets you change the XMPP Presence
        /// </summary>
        /// <param name="status">Your status</param>
        /// <param name="pres">The pressence</param>
        /// <param name="show">The show</param>
        public void SetPresence(string status, PresenceType pres, PresenceShow show)
        {
            var encodedXml = System.Security.SecurityElement.Escape(status);

            var presence =
                Encoding.UTF8.GetBytes(
                    $"<presence type=\"{ConvertPresenceType.ConvertPresenceTypeToString(pres)}\"><priority>0</priority><show>{ConvertPresenceShow.ConvertPresenceShowToString(show)}</show><status>{encodedXml}</status></presence>");
            SslStream.Write(presence);
        }

        /// <summary>
        /// Lets you change the XMPP Presence
        /// </summary>
        /// <param name="status">Your status</param>
        /// <param name="pres">The pressence</param>
        /// <param name="show">The show</param>
        /// <returns>The output presence</returns>
        public string PresenceAsString(string status, PresenceType pres, PresenceShow show)
        {
            var encodedXml = System.Security.SecurityElement.Escape(status);
            return $"<presence type=\"{ConvertPresenceType.ConvertPresenceTypeToString(pres)}\"><priority>0</priority><show>{ConvertPresenceShow.ConvertPresenceShowToString(show)}</show><status>{encodedXml}</status></presence>";
        }

        public void JoinRoom(Jid roomJid, string password)
        {
            var roomJoinString = Encoding.UTF8.GetBytes(
                $"<presence from=\'{MainJid.RawJid}\' to=\'{roomJid.RawJid}\'>" +
                "<x xmlns=\'http://jabber.org/protocol/muc\'>" +
                $"<password>{password}</password>" +
                "</x>" +
                "</presence>");
        }

        public void JoinRoom(Jid roomJid)
        {
            var roomJoinString = Encoding.UTF8.GetBytes(
                $"<presence from=\'{MainJid.RawJid}\' to=\'{roomJid.RawJid}\'>" +
                "<x xmlns=\'http://jabber.org/protocol/muc\'/>" +
                "</presence>");
            SslStream.Write(roomJoinString);
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        /// <summary>
        /// Send a message to a user
        /// </summary>
        /// <param name="to">The user's Jid you want to send the message to</param>
        /// <param name="message">The message to send</param>
        public void SendMessage(Jid to, string message)
        {
            //Set to high priority
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var encodedXml = System.Web.HttpUtility.HtmlEncode(message);
            var messageSend =
                Encoding.UTF8.GetBytes(
                    $"<message from=\'{MainJid.RawJid}\' to=\'{to.RawJid}\' type=\'chat\'><body>{encodedXml}</body></message>");
            SslStream.Write(messageSend);
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        public void SendGroupChatMessage(Jid to, string message)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var encodedXml = System.Web.HttpUtility.HtmlEncode(message);
            var messageSend =
                Encoding.UTF8.GetBytes(
                    $"<message from=\'{MainJid.RawJid}\' to=\'{to.RawJid}\' type=\'groupchat\'><body>{encodedXml}</body></message>");
            SslStream.Write(messageSend);
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        /// <summary>
        /// Sends the disconnect packet
        /// </summary>
        public void Disconnect()
        {
            var disStr = Encoding.UTF8.GetBytes( "</stream:stream>");
            SslStream.Write(disStr);
            Disconnecting = true;
        }

        private bool _isPinging = false;
        /// <summary>
        /// Handdles the XMPP Ping to stop disconnects
        /// </summary>
        private void Ping()
        {
            var pingThread = new Thread(() =>
            {
                _isPinging = true;
                while (!Disconnecting)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    if (GetChatPresence == null)
                        continue;
                    var messageSend =
                        Encoding.UTF8.GetBytes(GetChatPresence());
                    SslStream.Write(messageSend);
                }
            });
            if (!_isPinging)
            {
                pingThread.Start();
            }
        }

        /// <summary>
        /// The handler for when the string is recieved
        /// </summary>
        /// <param name="x">The recieved string</param>
        private bool ChatServer_RecieveString(string x)
        {
            //Lazy hack, this converts the initial starting output to a valid XMP
            if (x.Contains("<stream:stream"))
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
                        try
                        {
                            //Make sure that the presence is not from yourself. We already know you are online
                            if (el.Attributes["from"].Value == el.Attributes["to"].Value)
                                return true;

                            //Create the new presence
                            var pres = new Presence
                            {
                                FromJid = new Jid(el.Attributes["from"].Value),
                            };
                            try
                            {
                                //Get the presence type
                                pres.PresenceType =
                                    (PresenceType) Enum.Parse(typeof(PresenceType), el.Attributes["type"].Value, true);
                            }
                            catch
                            {
                                //Ignored
                            }

                            //Handle more presence data
                            foreach (var presData in el.ChildNodes)
                            {
                                var xmlPres = (XmlNode) presData;
                                if (xmlPres.Name == "show")
                                {
                                    pres.PresenceShow = (PresenceShow) Enum.Parse(typeof(PresenceShow),
                                        xmlPres.InnerText, true);
                                }
                                else if (xmlPres.Name == "status")
                                {
                                    pres.Status = System.Web.HttpUtility.HtmlDecode(xmlPres.InnerText);
                                }
                                else if (xmlPres.Name == "last_online")
                                {
                                    pres.LastOnline = xmlPres.InnerText;
                                }
                            }

                            //Handle if it should be mobile or a normal presence
                            if (pres.PresenceShow == PresenceShow.Mobile)
                            {
                                OnMobilePresenceRecieved?.Invoke(pres);
                            }
                            else
                            {
                                OnPlayerPresenceRecieved?.Invoke(pres);
                            }
                        }
                        catch
                        {
                            //Ignore for now
                        }
                    }

                    #endregion XmppPresenceHandler

                    #region XmppMessageRecieveHandler

                    else if (el.Name == "message" && el.HasChildNodes)
                    {
                        try
                        {
                            if (el.Attributes["from"].Value == el.Attributes["to"].Value)
                                return true;

                            OnMessageRecieved?.Invoke(new Jid(el.Attributes["from"].Value), el.InnerText);
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

                            if (xmlNode.Name == "mechanisms" && xmlNode.InnerText == "X-Riot-RSO")
                            {
                                //This converts the rso string to bytes to send
                                var authStr =
                                    Encoding.UTF8.GetBytes(
                                        $"<auth xmlns=\"urn:ietf:params:xml:ns:xmpp-sasl\" mechanism=\"X-Riot-RSO\">{RsoToken}</auth>");

                                SslStream.Write(authStr);
                                HandledRsoAuth?.Invoke(RsoToken);
                            }

                            #endregion AuthHandler

                            #region ConnectionSuccessHandler

                            else if (el.Name == "success" && xmlNode.Name == "#text")
                            {
                                //I have no idea why I did this, but I did it. Don't question me Wild
                                if (int.TryParse(xmlNode.InnerText, out var output))
                                    Id = output;

                                //Say we logged in okay
                                OnSuccessLogin?.Invoke();

                                //Send another random static string
                                var authStr =
                                    Encoding.UTF8.GetBytes(
                                        "<stream:stream to=\"pvp.net\" xml:lang=\"*\" version=\"1.0\" xmlns:stream=\"http://etherx.jabber.org/streams\" xmlns=\"jabber:client\">");
                                SslStream.Write(authStr);
                            }

                            #endregion ConnectionSuccessHandler

                            #region IDontKnowWtfThisIsButReturnBinding

                            else if (xmlNode.Name == "rxep")
                            {
                                //This is sent from client to server after this is recieved. I honestly don't know this xmpp stuff so lets pretend we read that
                                var resource =
                                    Encoding.UTF8.GetBytes(
                                        "<iq type=\"set\" id=\"0\"><bind xmlns=\"urn:ietf:params:xml:ns:xmpp-bind\"><resource>RC</resource></bind></iq>");

                                SslStream.Write(resource);

                                break;
                            }

                            #endregion IDontKnowWtfThisIsButReturnBinding

                            #region HandleSessionIQ

                            else if (el.Name == "iq" && xmlNode.Name == "bind" && xmlNode.HasChildNodes)
                            {
                                //Make sure that this is the correct IQ and stuff
                                if (xmlNode.ChildNodes.Count == 1 &&
                                    xmlNode.FirstChild.Name == "jid")
                                {
                                    MainJid = new Jid(xmlNode.InnerText);

                                    var session =
                                        Encoding.UTF8.GetBytes(
                                            "<iq type=\"set\" id=\"1\"><session xmlns=\"urn:ietf:params:xml:ns:xmpp-session\"/></iq>");

                                    SslStream.Write(session);

                                    Ping();
                                }
                            }

                            #endregion HandleSessionIQ

                            #region SessionIQHandler

                            else if (el.Name == "iq" && xmlNode.Name == "session")
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
                                        SslStream.Write(Encoding.UTF8.GetBytes(presence));
                                    }

                                    //Request roster and priv_req (I think this is friend requests)
                                    SslStream.Write(Encoding.UTF8.GetBytes(
                                        $"<iq type=\"get\" id=\"priv_req_2\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:privacy\"><list name=\"LOL\"/></query></iq>"));
                                    SslStream.Write(Encoding.UTF8.GetBytes(
                                        $"<iq type=\"get\" id=\"rst_req_3\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:roster\"/></iq>"));
                                    var date = "";

                                    //Lazy hack to subtract a month from today's date
                                    date = DateTime.Now.Month == 1
                                        ? $"{DateTime.Now.Year - 1}-12-{DateTime.Now.Day}"
                                        : $"{DateTime.Now.Year}-{DateTime.Now.Month - 1}-{DateTime.Now.Day}";

                                    //Retireve any former messages from the hisory of the archives
                                    SslStream.Write(Encoding.UTF8.GetBytes(
                                        $"<iq type=\"get\" id=\"recent_conv_req_4\" to=\"{MainJid.PlayerJid}\"><query xmlns=\"jabber:iq:riotgames:archive:list\">" +
                                        $"<since>{date} 00:00:00</since><count>10</count></query></iq>"));
                                }
                            }

                            #endregion SessionIQHandler

                            #region RosterIQ

                            else if (el.Name == "iq" && el.HasAttribute("id") &&
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
                                            var inJid = new Jid(itemRostNode.Attributes["jid"].Value)
                                            {
                                                SumName = itemRostNode.Attributes["name"].Value
                                            };

                                            //If the user has a group print it, otherwise return **Default
                                            inJid.Group = itemRostNode.HasChildNodes
                                                ? itemRostNode.FirstChild.InnerText
                                                : "**Default";

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

        public void StartReadLoop()
        {
            var t = new Thread(() =>
            {
                //Sometimes strings are sent fragmented. This stores the fragmented string
                string fragStr = string.Empty;

                //Create the data for the SslStream.Read
                var buffer = new byte[2048];
                var messageData = new StringBuilder();
                var bytes = -1;

                do
                {
                    if (Disconnecting)
                        break;
                    try
                    {
                        //Read
                        bytes = SslStream.Read(buffer, 0, buffer.Length);

                        //Decode that data
                        var decoder = Encoding.UTF8.GetDecoder();
                        var chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                        decoder.GetChars(buffer, 0, bytes, chars, 0);
                        messageData.Append(chars);

                        //Temp, log the data
                        Debugger.Log(0, "", messageData + "\n");
                        //Make sure that the message actually has content, or just ignore it
                        if (!string.IsNullOrWhiteSpace(messageData.ToString()))
                        {
                            //If the string ends with a > we know that it is is the end of the xmpp string that was received
                            //If not, add it to the string fragment helper
                            if (messageData.ToString().EndsWith(">"))
                            {
                                if (messageData.ToString().EndsWith(">"))
                                {
                                    if (RecieveString?.Invoke(fragStr + messageData) == true)
                                    {
                                        //Empty the string fragment helper for next time
                                        fragStr = string.Empty;
                                    }
                                }
                                else
                                {
                                    fragStr += messageData.ToString();
                                }
                            }
                            else
                            {
                                fragStr += messageData.ToString();
                            }
                        }

                        messageData.Clear();
                    }
                    catch
                    {
                        if (!Disconnecting)
                            throw;
                    }
                } while (bytes != 0);
            }) {Priority = ThreadPriority.AboveNormal};
            t.Start();
        }
    }
}
