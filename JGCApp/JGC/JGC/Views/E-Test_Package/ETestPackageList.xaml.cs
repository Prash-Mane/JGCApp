using JGC.DataBase.DataTables;
using JGC.ViewModels.E_Test_Package;
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
	public partial class ETestPackageList : ContentPage
	{
		public ETestPackageList ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (T_ETestPackages)obj.Parameter;
            var viewModel = (ETestPackageVewModel)BindingContext;
            viewModel.DeleteTestPackage(parameter);
        }
    }
}