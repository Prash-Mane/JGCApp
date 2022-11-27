using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.MasterPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterNavigationPage : NavigationPage
    {
        public static MasterNavigationPage Instance { get; private set; }
        public MasterNavigationPage()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}