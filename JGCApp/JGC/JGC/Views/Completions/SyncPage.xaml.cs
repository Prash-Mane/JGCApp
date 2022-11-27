using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncPage : PopupPage
    {
        public SyncPage()
        {
            InitializeComponent();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                // You're on a phone 
                FrameTagPopup.Scale = 0.9;
            }
            else
            {
                FrameTagPopup.HorizontalOptions = LayoutOptions.Center;
                // You're on a tablet
            }
            if (Device.RuntimePlatform == "UWP")
            {
                FrameTagPopup.Margin = new Thickness(50, 0, 50, 0);
                Padding = 20;
                FrameTagPopup.Padding = 20;
            }



            // if (Device.RuntimePlatform == 

        }


        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (Button)sender;
            var viewModel = (SyncViewModel)BindingContext;
            var workpack = (dynamic)item.CommandParameter;
            viewModel.OnClickWorkPackAdd(workpack);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return;
            var selected = e.Item;

            ((ListView)sender).SelectedItem = null; // de-select the row
        }
    }
}