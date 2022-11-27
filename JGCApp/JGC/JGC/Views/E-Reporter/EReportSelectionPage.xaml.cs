using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EReportSelectionPage : ContentPage
	{
        #region Private
        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
                                   // Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null;//e.Item; // de-select the row
        }
        #endregion
        #region Public
        public EReportSelectionPage()
        {
            InitializeComponent();
        }
        #endregion
    }
}