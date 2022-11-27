using System;
using System.Collections.Generic;
using JGC.Models;
using JGC.Models.E_Test_Package;
using JGC.ViewModels.E_Test_Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.E_Test_Package
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreTestRecordPage : ContentPage
    {
        public PreTestRecordPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var Control = sender.GetType().Name;
            // var  RC = (EventArgs)e;


            if (Control == "Button")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((Button)sender).CommandParameter;
                var viewModel = (PreTestRecordViewModel)BindingContext;
                viewModel.gvDrainRecordContent_CellContentClick("btnNA", SelectedRecord);
                viewModel.SelectedConfirmationSource = SelectedRecord;
            }
            else if (Control == "Image")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((TappedEventArgs)e).Parameter;
                var viewModel = (PreTestRecordViewModel)BindingContext;
                viewModel.gvDrainRecordContent_CellContentClick("Image", SelectedRecord);
                viewModel.SelectedConfirmationSource = SelectedRecord;
            }

            //var viewModel = (DrainRecordViewModel)BindingContext;
            //viewModel.gvDrainRecordContent_CellContentClick();


            //((ListView)sender).SelectedItem = null; // de-select the row
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var Control = sender.GetType().Name;

            var SelectedRecord = (RecordAcceptedBy)((TappedEventArgs)e).Parameter;
            var viewModel = (PreTestRecordViewModel)BindingContext;
            viewModel.gvDrainRecordAcceptedBy_CellContentClick("Image", SelectedRecord);
            viewModel.SelectedrecordAcceptedBySource = SelectedRecord;
        }

        private void TapGestureRecognizer_TappedDescription(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (PreTestRecordViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }
    }
}