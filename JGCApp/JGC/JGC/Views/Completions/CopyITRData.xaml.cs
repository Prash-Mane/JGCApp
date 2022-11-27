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
    public partial class CopyITRData : ContentPage
    {

        ViewCell TaglastCell;
        ViewCell ITRlastCell;
        public CopyITRData()
        {
            InitializeComponent();
        }

        private void TagViewCell_Tapped(object sender, EventArgs e)
        {
            //if (TaglastCell != null)
            //    TaglastCell.View.BackgroundColor = Color.Transparent;
            var viewModel = (CopyITRDataViewModel)BindingContext;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                if (Color.FromHex(viewCell.View.BackgroundColor.ToHex()) != Color.FromHex("#304D61"))
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#304D61");
                    viewModel.BtnClicked("AddTags");
                }
                else
                {
                    viewCell.View.BackgroundColor = Color.White;
                    viewModel.BtnClicked("RemoveTags");
                }
                viewCell.View.VerticalOptions = LayoutOptions.Fill;
                viewCell.View.HorizontalOptions = LayoutOptions.Fill;

                TaglastCell = viewCell;
            }
        }

        private void ITRViewCell_Tapped(object sender, EventArgs e)
        {
            //if (ITRlastCell != null)
            //    ITRlastCell.View.BackgroundColor = Color.Transparent;
            var viewModel = (CopyITRDataViewModel)BindingContext;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                if (Color.FromHex(viewCell.View.BackgroundColor.ToHex()) != Color.FromHex("#304D61"))
                {
                    viewCell.View.BackgroundColor = Color.FromHex("#304D61");
                    viewModel.BtnClicked("AddITRs");
                }
                else
                {
                    viewCell.View.BackgroundColor = Color.White;
                    viewModel.BtnClicked("RemoveITRs");
                }
                viewCell.View.VerticalOptions = LayoutOptions.Fill;
                viewCell.View.HorizontalOptions = LayoutOptions.Fill;

                ITRlastCell = viewCell;
            }
        }
    }
}