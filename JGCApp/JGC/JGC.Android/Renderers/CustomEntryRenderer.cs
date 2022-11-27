
using Android.Content;
using Xamarin.Forms.Platform.Android;
using JGC.Droid.Renderers;
using JGC.UserControls.CustomControls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace JGC.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.LightGray);
            }
        }
    }
}