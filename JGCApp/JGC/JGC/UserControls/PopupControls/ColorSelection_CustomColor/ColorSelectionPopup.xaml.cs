using JGC.Common.ColorChanged;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.UserControls.PopupControls.ColorSelection_CustomColor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorSelectionPopup : PopupPage
    {
        ColorSelectionViewModel ViewModel;
        readonly ColorPickerPopup _ColorPickPopup;

        public ColorSelectionPopup ()
		{
			InitializeComponent ();
            BindingContext = ViewModel = new ColorSelectionViewModel();
            //  ColorSelection
            _ColorPickPopup = new ColorPickerPopup();
            _ColorPickPopup.ColorChanged += CustomColorPickerOnColorChanged;
        }

        private async void SelectionClick(object sender, EventArgs e)
        {
            if (ViewModel.SelectedColor == null)
            {
                await DisplayAlert("No color selected", "You must choose one color !", "OK");
                return;
            }
            //Send a message to notify the selected Color
            MessagingCenter.Send(ViewModel, "_categoryColor");
            //close this
            await PopupNavigation.PopAsync(true);
        }

        private async void CustomClick(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);

            await Navigation.PushPopupAsync(_ColorPickPopup);
        }


        private async void Button_Pressed(object sender, EventArgs e)
        {
            if (ViewModel.SelectedColor == null)
            {
                await DisplayAlert("No color selected", "You must choose one color !", "OK");
                return;
            }
            //Send a message to notify the selected Color
            MessagingCenter.Send(ViewModel, "_categoryColor");
            //close this
            await PopupNavigation.PopAsync(true);
        }


        void CustomColorPickerOnColorChanged(object sender, ColorChangedEventArgs args)
        {
            //   BoxView.Color = args.Color;
            _ColorPickPopup.IsEnabled = true;
        }
    }
}