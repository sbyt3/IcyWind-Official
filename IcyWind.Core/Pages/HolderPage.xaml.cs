using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace IcyWind.Core.Pages
{
    /// <summary>
    ///     Interaction logic for MainControl.xaml
    /// </summary>
    public partial class HolderPage : UserControl
    {
        public HolderPage()
        {
            InitializeComponent();
        }

        public void ShowNotification(string message)
        {
            MessageText.Content = message;

            //232,0,232,10
            var moveAnimation = new ThicknessAnimation(new Thickness(232, 0, 232, 10), TimeSpan.FromSeconds(0.25));
            NotifyCardBottom.BeginAnimation(MarginProperty, moveAnimation);
            var t = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            t.Elapsed += (o, e) =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
                {
                    moveAnimation = new ThicknessAnimation(new Thickness(232, 0, 232, -22), TimeSpan.FromSeconds(0.25));
                    NotifyCardBottom.BeginAnimation(MarginProperty, moveAnimation);
                    t.Stop();
                }));
                
            };
            t.Start();

        }
    }
}