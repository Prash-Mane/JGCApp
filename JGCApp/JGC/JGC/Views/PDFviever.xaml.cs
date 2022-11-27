using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PDFviever : ContentPage
	{
		public PDFviever ()
		{
            InitializeComponent ();            
		}
        private void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}