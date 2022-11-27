using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.UserControls.PopupControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NetworkErrorPopup : PopupPage
    {
		public NetworkErrorPopup ()
		{
			InitializeComponent ();
		}
        private async void OnContinue(object sender, EventArgs e)
        {
            try
            {
                if (PopupNavigation.PopupStack.Count > 0)
                    await PopupNavigation.PopAllAsync(false);
            }
            catch
            {
               
            }
        }
    }
}