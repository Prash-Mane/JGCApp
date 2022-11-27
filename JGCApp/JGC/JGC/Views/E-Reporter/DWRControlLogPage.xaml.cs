using JGC.DataBase.DataTables;
using JGC.ViewModels;
using JGC.ViewModels.E_Reporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.E_Reporter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DWRControlLogPag : ContentPage
    {
        
        public DWRControlLogPag()
        {
           InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (e == null) return;
            var item = (TappedEventArgs)e;
            var viewModel = (DWRControlLogViewModel)BindingContext;
            viewModel.SelectedSignatureItem = (T_EReports_Signatures)item.Parameter;
            
            viewModel.UpdateSignatureItem(true);
          
        }
    }
}