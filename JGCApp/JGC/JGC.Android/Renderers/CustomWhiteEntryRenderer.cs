using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using JGC;
using JGC.Droid.Renderers;
using JGC.UserControls.CustomControls;

[assembly: ExportRenderer(typeof(CustomWhiteEntry), typeof(CustomWhiteEntryRenderer))]
namespace JGC.Droid.Renderers
{
    public class CustomWhiteEntryRenderer : EntryRenderer
    {
        public CustomWhiteEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.White);
            }

            e.NewElement.Unfocused += (sender, evt) =>
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.White);

            };
            e.NewElement.Focused += (sender, evt) =>
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.LightGray);
            };
        }
    }

}