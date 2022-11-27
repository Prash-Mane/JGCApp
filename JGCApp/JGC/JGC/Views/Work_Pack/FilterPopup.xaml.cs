using JGC.ViewModels.Work_Pack;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Work_Pack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class FilterPopup : PopupPage
    {
        public FilterPopup(FilterPopupViewModel vm, List<string> Source)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}