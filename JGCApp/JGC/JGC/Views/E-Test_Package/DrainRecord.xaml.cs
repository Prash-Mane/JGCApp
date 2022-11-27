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

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DrainRecord : ContentPage
	{
        List<TestRow> Testreports = new List<TestRow>
        {
            new TestRow{
            Numbers="1",
            DescriptionSign = "Check normimnated valves and instrument are moved or isolated for testing and flusing.",
            NameSigned = "",
            DateSigned = "",
            PICSigned = "TSV",
            SignOff = "N/A Sign Off"},

            new TestRow{
            Numbers="2",
            DescriptionSign = "Check test binds and temporary spools installed are correctly rated.",
            NameSigned = "",
            DateSigned = "",
            PICSigned = "TSV",
            SignOff = "N/A Sign Off"},

            new TestRow{
            Numbers="3",
            DescriptionSign = "Check the high points vents and low points drains, wheather it is applicable.",
            NameSigned = "",
            DateSigned = "",
            PICSigned = "TSV",
            SignOff = "N/A Sign Off"},

            new TestRow{
            Numbers="4",
            DescriptionSign = "Check all instruments on test maniflod are correctly rated and valid calibration certificate ",
            NameSigned = "",
            DateSigned = "",
            PICSigned = "TSV",
            SignOff = "N/A Sign Off"}

        };

  
        public DrainRecord ()
		{
			InitializeComponent ();
         //   this.BindingContext = Testreports;
          

        }
      
        public class TestRow
        {
            public string Numbers { get; set; }
            public string DescriptionSign { get; set; }
            public string NameSigned { get; set; }
            public string DateSigned { get; set; }
            public string Signedby { get; set; }
            public string PICSigned { get; set; }
            public string SignOff { get; set; }      
            public DataTemplate CellTemplate { get; set; }
            public string Error { get; set; }

        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            var Control = sender.GetType().Name;
           // var  RC = (EventArgs)e;
          

            if (Control == "Button")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((Button)sender).CommandParameter;
                var viewModel = (DrainRecordViewModel)BindingContext;
                viewModel.gvDrainRecordContent_CellContentClick("btnNA", SelectedRecord);
                viewModel.SelectedConfirmationSource = SelectedRecord;
            }
            else if (Control == "Image")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((TappedEventArgs)e).Parameter;
                var viewModel = (DrainRecordViewModel)BindingContext;
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
            var viewModel = (DrainRecordViewModel)BindingContext;
            viewModel.gvDrainRecordAcceptedBy_CellContentClick("Image", SelectedRecord);
            viewModel.SelectedrecordAcceptedBySource = SelectedRecord;
        }
        private void TapGestureRecognizer_TappedDescription(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (DrainRecordViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }
    }
}