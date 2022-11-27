using JGC.Common.Interfaces;
using JGC.UWP.DependancyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace JGC.UWP.DependancyObjects
{
    public class ToastMessage : IToastMessage
    {
        public void LongAlert(string message)
        {
            Show(message);
        }

        public void ShortAlert(string message)
        {
            Show(message);
        }

        public void Show(string message)
        {
            var label = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Windows.UI.Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            var style = new Style { TargetType = typeof(FlyoutPresenter) };
            style.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.Black)));
            style.Setters.Add(new Setter(FrameworkElement.MaxHeightProperty, 1));
            var flyout = new Flyout
            {
                Content = label,
                Placement = FlyoutPlacementMode.Full,
                FlyoutPresenterStyle = style,
            };

            flyout.ShowAt(Window.Current.Content as FrameworkElement);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.0) };
            timer.Tick += (sender, e) => {
                timer.Stop();
                flyout.Hide();
            };
            timer.Start();
        }
    }
}
