using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Work_Pack
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IWPSelectionPage : ContentPage
	{
		public IWPSelectionPage ()
		{
			InitializeComponent ();
		}

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width != SearchGrid.WidthRequest || height != SearchGrid.HeightRequest)
            {
                if (width > height)
                {
                    SearchGrid.WidthRequest = width;
                    if (Device.RuntimePlatform == Device.UWP)
                        SearchGrid.HeightRequest = 175;                    
                    else
                        SearchGrid.HeightRequest = 75;
                }
                else
                {
                    SearchGrid.WidthRequest = width;
                    if (Device.RuntimePlatform == Device.UWP)
                        SearchGrid.HeightRequest = 175;
                    else
                        SearchGrid.HeightRequest = 210;
                }
            }
        }
    }
}