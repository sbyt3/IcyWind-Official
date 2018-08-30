using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IcyWind.Core.Controls
{
    public class FadeLabel : Label
    {
        public byte HoverColor = 236;
        public byte NoHoverColor = 159;
        public bool KeepColor;

        public FadeLabel()
        {
            MouseEnter += FadeLabel_MouseEnter;
            MouseLeave += FadeLabel_MouseLeave;
            Foreground = new SolidColorBrush(Color.FromRgb(NoHoverColor, NoHoverColor, NoHoverColor));
        }

        public FadeLabel(byte hoverColor, byte noHoverColor)
        {
            HoverColor = hoverColor;
            NoHoverColor = noHoverColor;
            MouseEnter += FadeLabel_MouseEnter;
            MouseLeave += FadeLabel_MouseLeave;
            Foreground = new SolidColorBrush(Color.FromRgb(noHoverColor, noHoverColor, noHoverColor));
        }

        private void FadeLabel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (KeepColor)
                return;

            FadeOut();
        }

        public void FadeOut()
        {
            var changeColorAnimation = new ColorAnimation(Color.FromRgb(NoHoverColor, NoHoverColor, NoHoverColor), TimeSpan.FromSeconds(0.5));
            var s = new Storyboard {Duration = new Duration(new TimeSpan(0, 0, 1))};
            s.Children.Add(changeColorAnimation);
            Storyboard.SetTarget(changeColorAnimation, this);
            Storyboard.SetTargetProperty(changeColorAnimation, new PropertyPath("Foreground.Color"));
            s.Begin();
        }

        void FadeLabel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var changeColorAnimation = new ColorAnimation(Color.FromRgb(HoverColor, HoverColor, HoverColor), TimeSpan.FromSeconds(0.5));
            var s = new Storyboard {Duration = new Duration(new TimeSpan(0, 0, 1))};
            s.Children.Add(changeColorAnimation);
            Storyboard.SetTarget(changeColorAnimation, this);
            Storyboard.SetTargetProperty(changeColorAnimation, new PropertyPath("Foreground.Color"));
            s.Begin();
        }
    }
}
