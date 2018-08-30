using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot;
using IcyWind.Core.Logic.Riot.Lobby;
using Newtonsoft.Json;

namespace IcyWind.Core.Pages.IcyWindPages.PlayPage
{
    /// <summary>
    /// Interaction logic for MapSelectionPage.xaml
    /// </summary>
    public partial class MapSelectionPage : UserControl
    {
        private List<int> enabledQueues = new List<int>();
        public MapSelectionPage()
        {
            InitializeComponent();
            QueueButtonBox.Items.Clear();
            QueueButtonBox2.Items.Clear();

            var mapList = new List<int>();

            var activeQueues = StaticVars.ActiveClient.RiotQueues.Where(x => x.QueueState == "ON");

            foreach (var queues in activeQueues)
            {
                foreach (var mapId in queues.SupportedMapIds)
                {
                    if (!mapList.Contains((int) mapId))
                    {
                        mapList.Add((int)mapId);
                    }
                }

                enabledQueues.Add((int)queues.Id);

            }

            foreach (var map in mapList)
            {

                var name = QueueConverter.MapToName((Map) map);

                var mapView = new MapView
                {
                    NameLabel = {Content = name},
                    MapImage =
                    {
                        Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation,
                            "IcyWindAssets", "Maps", $"map{map}.png")))
                    },
                    Tag = map
                };

                mapView.MouseDown += MapView_MouseDown;

                MapListView.Items.Add(mapView);
            }

        }

        private void MapView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            QueueButtonBox.Items.Clear();
            QueueButtonBox2.Items.Clear();
            var queues = QueueConverter.Converter.Where(x => x.Value.Key == ((Map) ((MapView) e.Source).Tag))
                .Where(x => enabledQueues.Any(c => c == x.Key));

            foreach (var queue in queues)
            {
                var button = new Button
                {
                    Content = queue.Value.Value,
                    Tag = queue.Key
                };
                button.Click += ButtonOnClick;

                if (queue.Value.Value.Contains("Ranked"))
                    button.IsEnabled = false;
                
                if (QueueButtonBox.Items.Count < 5)
                    QueueButtonBox.Items.Add(button);
                else
                {
                    QueueButtonBox2.Items.Add(button);
                }
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {

            if (e.Source is Button butt)
            {
                if ((int) butt.Tag != 430)
                {
                    UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NotFinishedFeature"));
                    return;
                }

                UserInterfaceCore.ChangeMainPageView<LobbyPage>(butt.Tag);
            }
        }
    }
}
