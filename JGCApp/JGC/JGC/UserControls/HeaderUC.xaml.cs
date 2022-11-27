using JGC.Common.Helpers;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;


namespace JGC.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderUC 
	{
        public HeaderUC ()
		{
			InitializeComponent ();
            if (Settings.Report != null && Settings.Report != "")
            {
                search.IsVisible = true;
            }
            else
                search.IsVisible = false;

            //if (true)
            //    PunchOverview.IsVisible = PunchView.IsVisible = ControlLog.IsVisible = Pandid.IsVisible = TestRecord.IsVisible = DrainRecord.IsVisible = false;

            if (Settings.ModuleName != null)
            {
                lblEdit.TextColor = Color.FromHex("#1A5276");
                if (Settings.ModuleName == "EReporter")
                {
                    Logo.Source = "EReporter_logo.png";
                    searchcommand.CommandParameter = "DashboardPage";
                }                    
                else if (Settings.ModuleName == "TestPackage")
                {
                    Logo.Source = "TestPackage_logo.png";
                    searchcommand.CommandParameter = "ETestPackageList";
                }
                else if (Settings.ModuleName == "JobSetting")
                {
                    Logo.Source = "WorkPackage_logo.png";
                    searchcommand.CommandParameter = "IWPSelectionPage";
                }
                else if (Settings.ModuleName == "Completions")
                {
                    lblEdit.TextColor = Color.FromHex("#1AA3B3");
                    Logo.Source = "Completions_logo.png";
                }

            }
        }

    }
}