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
    public partial class CompletionProjectList : ContentPage
    {
        ViewCell lastCell;
        public CompletionProjectList()
        {
            InitializeComponent();
        }
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;

            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#304D61");
                viewCell.View.VerticalOptions = LayoutOptions.Fill;
                viewCell.View.HorizontalOptions = LayoutOptions.Fill;

                lastCell = viewCell;
            }
        }
    }
}