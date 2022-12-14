using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Work_Pack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IWPPdfPage : ContentPage
    {
        public IWPPdfPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                pdfView.On<Android>().EnableZoomControls(true);
                pdfView.On<Android>().DisplayZoomControls(false);
            }
        }
    }
}