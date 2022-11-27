using JGC.Models.E_Test_Package;
using JGC.ViewModels.E_Test_Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.E_Test_Package
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class ControlLogPage : ContentPage
	{
		public ControlLogPage ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter =(ControlLogModel) obj.Parameter;
            var viewModel = (ControlLogViewModel)BindingContext;
            //if (viewModel.BtnCommand.CanExecute(parameter))
            //    viewModel.BtnCommand.Execute(parameter);
            try
            {
                viewModel.ControlLog_SignClick(parameter);
            }
            catch(Exception ex)
            {
            
            }
        }
       
            
    }
}