using Android.Content;
using JGC.Droid.Renderers;
using JGC.UserControls.CustomControls;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPDF), typeof(CustomPDFRenderer))]
namespace JGC.Droid.Renderers
{
    public class CustomPDFRenderer : WebViewRenderer
    {

        public CustomPDFRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            var customWebView = Element as CustomPDF;
            if (customWebView.Uri == null)
            {
                return;
            }
            try
            {
                
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.UseWideViewPort = true;
                Control.Settings.BuiltInZoomControls = true;
                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", GetFileNameFromUri(customWebView.Uri)))); 
            }
            catch(Exception ex)
            {
            }
            
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != CustomPDF.UriProperty.PropertyName)
            {
                return;
            }

            var customWebView = Element as CustomPDF;

            if (customWebView.Uri == null)
            {
                return;
            }
            Control.Settings.AllowUniversalAccessFromFileURLs = true;
            Control.Settings.UseWideViewPort = true;
            Control.Settings.BuiltInZoomControls = true;

            Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file://{0}", GetFileNameFromUri(customWebView.Uri))));
        }
        private static string GetFileNameFromUri(string uri)
        {
            return uri.Replace(" ", "%20");
        }
       

    }
}