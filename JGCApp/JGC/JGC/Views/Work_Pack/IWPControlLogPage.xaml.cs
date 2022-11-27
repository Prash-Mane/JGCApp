using JGC.Models.Work_Pack;
using JGC.ViewModels.Work_Pack;
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
	public partial class IWPControlLogPage : ContentPage
	{
		public IWPControlLogPage ()
		{
			InitializeComponent ();
		}
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (IWPControlLogModel)obj.Parameter;
            var viewModel = (IWPControlLogViewModel)BindingContext;
            if (viewModel.BtnCommand.CanExecute(parameter))
                viewModel.BtnCommand.Execute(parameter);
            viewModel.ControlLog_SignClick(parameter);
        }
    }
}