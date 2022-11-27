using JGC.UserControls.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Models
{
    public class PDFModel : ContentPage
    {
        public PDFModel()
        {          
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(9, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                },
            };
            grid.Children.Add(new CustomPDF
            {
                Uri = "sample.pdf",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            });

            grid.Children.Add(new Button
            {
                Text = "Back",
            }, 0, 1);

        }
    }
}
