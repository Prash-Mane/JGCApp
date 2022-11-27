using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfCommentPopup : PopupPage
    {
        public PdfCommentPopup(RejectPopupViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}