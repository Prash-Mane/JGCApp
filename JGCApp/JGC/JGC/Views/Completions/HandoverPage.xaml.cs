using JGC.Models.Completions;
using JGC.ViewModels.Completions;
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
    public partial class HandoverPage : ContentPage
    {
        HandoverViewModel hvm;
        public HandoverPage()
        {
            InitializeComponent();
            hvm = (HandoverViewModel)this.BindingContext;
        }
        [Obsolete]
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ImageData param = (ImageData)((TappedEventArgs)e).Parameter;
            if (param != null)
                hvm.OnTapedAsync(param);
        }
    }
}