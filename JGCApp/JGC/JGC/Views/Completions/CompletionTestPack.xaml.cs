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
    public partial class CompletionTestPack : ContentPage
    {
        CompletionTestPackViewModel Tvm;
        public CompletionTestPack()
        {
            InitializeComponent();
            Tvm = (CompletionTestPackViewModel)this.BindingContext;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            dynamic param = ((TappedEventArgs)e).Parameter;
            if (param != null)
                Tvm.OnTapedAsync(param);
        }
    }
}