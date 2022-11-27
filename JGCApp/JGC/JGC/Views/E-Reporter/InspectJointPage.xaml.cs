using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.E_Reporter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspectJointPage : ContentPage
    {
        private double width, height; 
        public InspectJointPage()
        {
            InitializeComponent();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        LeftFillCapGrid.WidthRequest = width * 0.6;
                        RightFillCapGrid.WidthRequest = width * 0.4;
                        TopLayout.Orientation = BottomLayout.Orientation = StackOrientation.Vertical;
                    }
                    else
                    {
                        RootGrid.WidthRequest = width * 0.4;
                        FillCapGrid.WidthRequest = width * 0.6;
                        LeftFillCapGrid.WidthRequest = FillCapGrid.WidthRequest * 0.6;
                        RightFillCapGrid.WidthRequest = FillCapGrid.WidthRequest * 0.4;
                        TopLayout.Orientation = BottomLayout.Orientation = StackOrientation.Horizontal;
                    }
                }
                else
                {
                    LeftFillCapGrid.WidthRequest = width * 0.6;
                    RightFillCapGrid.WidthRequest = width * 0.4;
                    TopLayout.Orientation = BottomLayout.Orientation = StackOrientation.Vertical;
                }
            }
        }
    }
}