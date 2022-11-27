using JGC.DataBase.DataTables.WorkPack;
using JGC.ViewModels.Work_Pack;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace JGC.Views.Work_Pack
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManPowerResourcePage : ContentPage
	{
        private double width, height;
        ZXingScannerPage scanPage;
        public ManPowerResourcePage ()
		{
			InitializeComponent ();         
        }

        private void Tap_DeleteWorkerItem(object sender, System.EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (ManPowerResourceViewModel)BindingContext;
            viewModel.SelectedWorker= (T_WorkerScanned)item.Parameter;
            viewModel.NavigateToRemoveWorkerItem();
        }

        private void Tap_DeleteAssignedWorkerItem(object sender, System.EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (ManPowerResourceViewModel)BindingContext;
            viewModel.SelectedAssignedWorker = (T_ManPowerLog)item.Parameter;
            viewModel.NavigateToRemoveAssignedWorkerItem();
        }
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ClickedOnScanCode(object sender, System.EventArgs e)
        {
            var customOverlay = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End
            };
            var Back = new Button
            {
                Text = "Back",
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                BackgroundColor = Color.Black,
            };
            Back.Clicked += Back_Clicked;
            customOverlay.Children.Add(Back);

            scanPage = new ZXingScannerPage(customOverlay: customOverlay);
            string ScanResult = string.Empty;

            scanPage.OnScanResult += (result) => {
            scanPage.IsScanning = false;
            

                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopModalAsync();
                    var viewModel = (ManPowerResourceViewModel)BindingContext;
                    viewModel.ScanQRCode(result.Text);
                });
            };
            await Navigation.PushModalAsync(scanPage);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    outerStack.Orientation = StackOrientation.Horizontal;
                    Addbtn.WidthRequest = -1;
                    WorkerList.HeightRequest = AssignedList.HeightRequest =  width;
                    Addbtn.Margin = new Thickness(0, 60, 0, 0);
                }
                else
                {
                    outerStack.Orientation = StackOrientation.Vertical;
                    Addbtn.WidthRequest = 200;                   
                    WorkerList.HeightRequest = AssignedList.HeightRequest = width/2;
                    Addbtn.Margin = new Thickness(0, 0, 0, 0);
                }
            }
        }


    }
}