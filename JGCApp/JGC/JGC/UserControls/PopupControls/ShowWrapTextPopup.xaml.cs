using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace JGC.UserControls.PopupControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShowWrapTextPopup : PopupPage
    {
		public ShowWrapTextPopup (string title,string Text)
		{
            try
            {
                InitializeComponent();
                Title.Text = title;
                label.Text = Text;
            }
            catch(Exception ex)
            {

            }
        }

        private async void OnClose(object sender, EventArgs e)
        {
            if (PopupNavigation.PopupStack.Count > 0)
                await PopupNavigation.PopAllAsync();
        }
    }
}