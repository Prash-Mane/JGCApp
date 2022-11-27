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
	public partial class DWR_EReporterPage : ContentPage
	{
		public DWR_EReporterPage()
		{
			InitializeComponent ();

            
            // Size size = Device.Info.PixelScreenSize;

            //if (size.Width >=1081)
            //{
            //    //var grid = s;
            //  //  Pare
            //  // stkLayoutTop.Margin = new Thickness(5, 30, 10, 0);
            //}
            //else
            //{
            //    //stkLayoutTop.Margin = new Thickness(5, 50, 10, 0);
            //}
          
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return;
            var selected = e.Item;
            var viewModel = (DWR_EReporterViewModel)BindingContext;
            viewModel.ShowBasemetal1 = viewModel.ShowBasemetal2 = viewModel.ShowHeatNos1 = viewModel.ShowHeatNos2 = false;
            viewModel.BtnHeatNosText1 = viewModel.BtnHeatNosText2 = viewModel.BtnMetalText1 = viewModel.BtnMetalText2 = "Pre-set";
            viewModel.SelectedBaseMetal1Text = viewModel.SelectedBaseMetal2Text = viewModel.SelectedHeatNos1Text = viewModel.SelectedHeatNos2Text = "";
            if (viewModel.BtnCommand.CanExecute("Editdetails"))
                viewModel.BtnCommand.Execute("Editdetails");

            //listView.SelectedItem = null;
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (e == null) return;
            var item =(TappedEventArgs) e;
            var viewModel = (DWR_EReporterViewModel)BindingContext;
            viewModel.SelectedSignatureItem = (T_EReports_Signatures) item.Parameter;
            viewModel.UpdateSignatureItem();
     
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           
            if (e == null) return;
            var item = (DWRRow)e.SelectedItem;
            if (item == null)
            {
                ((ListView)sender).SelectedItem = null;
                return;
            }

            var viewModel = (DWR_EReporterViewModel)BindingContext;
          
                viewModel.SelectedDWRRow = item;
                if (viewModel.NextBtnCommand.CanExecute("Step1"))
                    viewModel.NextBtnCommand.Execute("Step1");
               

        }

        private void BtnClearListSelection(object sender, EventArgs e)
        {
           // listView.SelectedItem = null;
         
        }


    }
}