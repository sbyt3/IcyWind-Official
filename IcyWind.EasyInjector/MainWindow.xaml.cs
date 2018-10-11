using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace IcyWind.EasyInjector
{
    public class EasyInjectorInterface : MarshalByRefObject
    {
        public void IsInstalled(int clientPID)
        {
            ReportMessage($"Debug: FakRiotLCU has injected FakLcuHook into process with PID: {clientPID}.");
        }

        public void ReportMessages(string[] messages)
        {
            foreach (var t in messages)
            {
                ReportMessage(t);
            }
        }

        public int GetPid { get; set; }

        public void ReportMessage(string message)
        {
            if (message.StartsWith("Read:"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    var item = new ReadWriteListItem($"[{DateTime.Now}] Read:", message.Remove(0, 5));
                    Holders.Data.Items.Add(item);
                }));
            }
            else if (message.StartsWith("Write:"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    var item = new ReadWriteListItem($"[{DateTime.Now}] Write:", message.Remove(0, 6));
                    Holders.Data.Items.Add(item);
                }));
            }
            else if (message.StartsWith("Debug:"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    var item = new ReadWriteListItem($"[{DateTime.Now}] Debug Message",
                        message.Remove(0, 6));
                    Holders.Data.Items.Add(item);
                }));
            }
        }

        public void ReportException(Exception e)
        {
            ReportMessage("The target process has reported an error:\r\n" + e);
        }

        public void Ping()
        {
        }
    }

    public static class Holders
    {
        public static ListBox Data;
    }

    public class ReadWriteListItem
    {
        public ReadWriteListItem(string header, string data)
        {
            Header = header;
            Data = data;
        }

        public string Header { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return Header;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
