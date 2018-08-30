using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using IcyWind.Chat;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.Chat;

namespace IcyWind.Core.Controls
{
    /// <summary>
    /// Interaction logic for ChatBoxControl.xaml
    /// </summary>
    public partial class ChatBoxControl : UserControl
    {
        public bool Visable { get; set; }
        private readonly Jid _jid;
        private readonly ChatPlayerItem _chatPlayerItem;
        public ChatBoxControl(Jid jid)
        {
            _jid = jid;
            _chatPlayerItem = StaticVars.ActiveClient.Players.First(x => x.JidAsString == jid.PlayerJid);
            InitializeComponent();
            foreach (var pastMessages in _chatPlayerItem.Messages)
            {
                if (pastMessages.Key == _jid.PlayerJid)
                {
                    AppendText($"{_chatPlayerItem.Username}: ", Brushes.DarkBlue);
                    AppendText(pastMessages.Value, Brushes.Black);
                    AppendText(Environment.NewLine, Brushes.Black);
                }
                else
                {
                    AppendText("You: ", Brushes.DarkOrchid);
                    AppendText(pastMessages.Value, Brushes.Black);
                    AppendText(Environment.NewLine, Brushes.Black);
                }
            }
        }

        private void SendText_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StaticVars.ActiveClient.XmppClient.SendMessage(_jid, SendText.Text);
                AppendText("You: ", Brushes.DarkOrchid);
                AppendText(SendText.Text, Brushes.Black);
                AppendText(Environment.NewLine, Brushes.Black);
                _chatPlayerItem.Messages.Add(new KeyValuePair<string, string>(StaticVars.ActiveClient.XmppClient.MainJid.PlayerJid, SendText.Text));
                SendText.Text = string.Empty;
            }
        }

        public void AppendText(string text, Brush color)
        {
            Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                var tr = new TextRange(ChatHistory.Document.ContentEnd, ChatHistory.Document.ContentEnd) {Text = text};
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }));
        }

        public void OnMessage(Jid jid, string message)
        {
            if (jid.PlayerJid == _jid.PlayerJid)
            {
                Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                {
                    AppendText($"{_chatPlayerItem.Username}: ", Brushes.DarkBlue);
                    AppendText(message, Brushes.Black);
                    AppendText(Environment.NewLine, Brushes.Black);
                }));
            }
        }
    }
}
