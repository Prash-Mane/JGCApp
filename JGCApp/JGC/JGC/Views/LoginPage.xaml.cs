
using JGC.Common.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Viwes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }      
        
    }
}