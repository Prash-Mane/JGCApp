using JGC.Common.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.MasterPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPage : MasterDetailPage
    {
        public MainMasterDetailPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Cache.MasterPage = this;
        }
    }
}