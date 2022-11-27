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
    public partial class DWRDrawingPage : ContentPage
    {
        private double width, height;
        public DWRDrawingPage()
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
                DrwaingsControl.WidthRequest = width;
                DrwaingsControl.HeightRequest = height / 1.5;
              
            }
        }
    }
}
