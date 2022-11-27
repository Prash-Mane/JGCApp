using JGC;
using JGC.UserControls.CustomControls;
using JGC.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomWhiteEntry), typeof(CustomWhiteEntryRenderer))]
namespace JGC.UWP.Renderers
{
    public class CustomWhiteEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = new SolidColorBrush(Colors.White);
                Control.BackgroundFocusBrush = new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}
