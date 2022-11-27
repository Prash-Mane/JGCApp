using JGC.ViewModels.E_Reporter;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.E_Reporter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignOffPopup : PopupPage
    {
        public SignOffPopup(SignOffPopupViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}