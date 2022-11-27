using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RejectPopup : PopupPage
    {
        public RejectPopup(RejectPopupViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            App.IsBusy = false;
        }
    }
}