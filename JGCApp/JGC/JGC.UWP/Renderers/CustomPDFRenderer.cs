using System;
using JGC.UWP.Renderers;
using JGC.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.Net;
using JGC.UserControls.CustomControls;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

[assembly: ExportRenderer(typeof(CustomPDF), typeof(CustomPDFRenderer))]

namespace JGC.UWP.Renderers
{
    public class CustomPDFRenderer : WebViewRenderer
    {
        protected async override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            CustomPDF customWebView = Element as CustomPDF;
            if (customWebView == null)
            {
                return;
            }
            if (customWebView.Uri == null)
            {
                return;
            }
           // Control.Source = new Uri("ms-appx-web:///Assets/pdfjs/web/viewer.html");
           // Control.LoadCompleted += Control_LoadCompleted;
            

            //Control.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", string.Format("ms-appx-web:///Assets/Content/sample.pdf")));

        }

        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != CustomPDF.UriProperty.PropertyName)
            {
                return;
            }

            var customWebView = Element as CustomPDF;
            if (customWebView == null)
            {
                return;
            }

            //if (customWebView.Uri == null)
            //{
            //    return;
            //}

            Control.Source = new Uri("ms-appx-web:///Assets/pdfjs/web/viewer.html");
            Control.LoadCompleted += Control_LoadCompleted;

            
            //Control.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file=ms-appx-web:///{0}", customWebView.Uri));

            // Control.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", string.Format("ms-appx-web:///Assets/Content/sample.pdf")));
        }
     
        private async Task<string> OpenAndConvert(string FileName)
        {
            try
            {
                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(FileName);
                var filebuffer = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(filebuffer.GetInputStreamAt(0));
                var bytes = new byte[filebuffer.Size];
                await reader.LoadAsync((uint)filebuffer.Size);
                reader.ReadBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async void Control_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            CustomPDF pdfView = Element as CustomPDF;
            //if (string.IsNullOrEmpty(pdfView?.Uri)) return;

            //var ret = await OpenAndConvert(pdfView?.Uri);
            //var jsfunction = $"window.openPdfAsBase64('{ret}')";

            //var obj = await Control.InvokeScriptAsync("eval", new[] { jsfunction });

            if (string.IsNullOrEmpty(pdfView?.Uri)) return;

            var ret = await OpenAndConvert(pdfView?.Uri);

            if(ret!= null)
                await Control.InvokeScriptAsync("openPdfAsBase64", new[] { ret });
        }
    }
}
