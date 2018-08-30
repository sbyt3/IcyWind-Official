using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
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
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Pages.IcyWindPages.PlayPage;

namespace IcyWind.Core.Pages
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : UserControl
    {
        public PlayPage()
        {
            InitializeComponent();
            //GetQueues();
            MapSelection.Content = new MapSelectionPage();
        }
    }
}
