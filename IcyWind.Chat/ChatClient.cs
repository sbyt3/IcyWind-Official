/*
 *  IcyWind.Chat - A simple XMPP chat library
 *  Copyright (C) 2018  IcyWind Software
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *  
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using IcyWind.Chat.Auth;
using IcyWind.Chat.Iq;
using IcyWind.Chat.Jid;
using IcyWind.Chat.Presence;
using IcyWind.Chat.TcpConnection;

namespace IcyWind.Chat
{
    public class ChatClient
    {
        #region XMPPClientData
        internal AuthHandler AuthHandler { get; set; }
        internal TcpStringClient TcpClient { get; set; }
        public PresenceManager PresenceManager { get; internal set; }
        public IqHandler IqManager { get; internal set; }
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
        /// Returned when the server returns a successful login
        /// </summary>
        public delegate void SuccessLogin();
        public event SuccessLogin OnSuccessLogin;
        #endregion Delegates

        #region PublicVars
        /// <summary>
        /// The current JID (Your JID)
        /// </summary>
        public UserJid MainJid { get; internal set; }
        #endregion PublicVars

        public ChatClient(IPEndPoint endPoint)
        {
            TcpClient = new TcpStringClient(endPoint, true);
        }

        public ChatClient(IPEndPoint endPoint, bool ssl)
        {
            TcpClient = new TcpStringClient(endPoint, ssl);
        }

        public async Task ConnectSSL(string host, AuthCred cred)
        {
            PresenceManager = new PresenceManager(this);
            IqManager = new IqHandler(this);
            await TcpClient.ConnectSSL(host);
            TcpClient.SendString(
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

            TcpClient.SendString(
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

            TcpClient.SendString($"<presence from=\'{MainJid.RawJid}\' to=\'{roomJid.RawJid}\'>" +
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

            TcpClient.SendString($"<message from=\'{MainJid.RawJid}\' to=\'{to.RawJid}\' type=\'chat\'><body>{encodedXml}</body></message>");
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }

        /// <summary>
        /// Sends the disconnect packet
        /// </summary>
        public void Disconnect()
        {
            TcpClient.SendString("</stream:stream>");
            Disconnecting = true;
        }

        /// <summary>
        /// The handler for when the string is received
        /// </summary>
        /// <param name="x">The received string</param>
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
                                TcpClient.SendString(
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

                                TcpClient.SendString("<iq type=\"set\" id=\"0\"><bind xmlns=\"urn:ietf:params:xml:ns:xmpp-bind\"><resource>RC</resource></bind></iq>");

                                break;
                            }

                            #endregion IDontKnowWtfThisIsButReturnBinding

                            #region IQManager
                            if (el.Name == "iq")
                            {
                                IqManager.HandleIq(xmlNode, el);
                            }
                            #endregion IQManager
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
