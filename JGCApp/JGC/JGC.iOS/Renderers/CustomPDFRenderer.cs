using System.IO;
using System.Net;
using Foundation;
using JGC.iOS.Renderers;
using JGC.UserControls.CustomControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPDF), typeof(CustomPDFRenderer))]
namespace JGC.iOS.Renderers
{
    public class CustomPDFRenderer : ViewRenderer<CustomPDF, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomPDF> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var customPDF = Element as CustomPDF;
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", WebUtility.UrlEncode(customPDF.Uri)));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
                Control.ScalesPageToFit = true;
            }
        }
    }
}