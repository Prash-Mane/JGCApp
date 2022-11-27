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
	
        public partial class PunchOverviewPage : ContentPage
        {
            List<RIRRow> RIRreports = new List<RIRRow>
        {
            new RIRRow{
            Description = "",
            Status = "Active",
            Signed = "",
            UpdateSigned = "" },

            new RIRRow{
            Description = "No Comment",
            Status = "Close",
            Signed = "",
            UpdateSigned = "" },

            new RIRRow{
            Description = "Lack of manitenance access/space for filters,strainers,instruments & valves",
            Status = "Open",
            Signed = "",
            UpdateSigned = "" }
        };
        public PunchOverviewPage()
        {
            InitializeComponent();
            this.BindingContext = RIRreports;
        }
        public class RIRRow
        {
            public string Description { get; set; }
            public string Status { get; set; }
            public string Signed { get; set; }
            public string UpdateSigned { get; set; }
            public DataTemplate CellTemplate { get; set; }
            public string Error { get; set; }

        }

        private void TapGestureRecognizer_TappedDescription(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (PunchOverviewViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

            var param = ((Xamarin.Forms.TappedEventArgs)e).Parameter;
            
            var viewModel = (PunchOverviewViewModel)BindingContext;
            viewModel.SelectedPunchOverview = (PunchOverview)param;
            viewModel.GotoPunchView();
        }
    }

}