using JGC.DataBase.DataTables;
using JGC.Models;
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
	public partial class MRR_EReporterPage : ContentPage
	{
		public MRR_EReporterPage ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (MRR_EReporterViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }
        private void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (MRR_EReporterViewModel)BindingContext;
            viewModel.SelectedSignatureItem = (T_EReports_Signatures)item.Parameter;

          
            viewModel.UpdateSignatureItem(true);
        }

        private void TapGestureRecognizer_TappedDelete(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (MRR_EReporterViewModel)BindingContext;
            viewModel.SelectedStorageArea = (MRRStorageAreas)item.Parameter;
            viewModel.NavigateToRemoveStorageAreas();
        }

        private void TapGestureRecognizer_TappedHeatDelete(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (MRR_EReporterViewModel)BindingContext;
            viewModel.SelectedHeatNo = (MRRHeatNos)item.Parameter;
            viewModel.NavigateToRemoveHeatNo();
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