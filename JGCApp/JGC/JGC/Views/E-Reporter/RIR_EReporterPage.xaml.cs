using JGC.DataBase.DataTables;
using JGC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RIR_EReporterPage : ContentPage
    {
		public RIR_EReporterPage()
		{
			InitializeComponent ();
		}
        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
                                   // Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (RIR_EReporterViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }

        private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (RIR_EReporterViewModel)BindingContext;
            viewModel.SelectedSignatureItem = (T_EReports_Signatures)item.Parameter;
            viewModel.UpdateSignatureItem(true);
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != CaptureImage.WidthRequest || height != CaptureImage.HeightRequest)
            {
                CaptureImage.WidthRequest = width;
                CaptureImage.HeightRequest = height;
               
            }
            if (width != AttachmentImage.WidthRequest || height != AttachmentImage.HeightRequest)
            {
                AttachmentImage.WidthRequest = width/2;
                AttachmentImage.HeightRequest = height/2;
            }
            if (width != Attachment.WidthRequest || height != Attachment.HeightRequest)
            {
                Attachment.WidthRequest = width;
                Attachment.HeightRequest = height;
            }
            
        }
    }
}