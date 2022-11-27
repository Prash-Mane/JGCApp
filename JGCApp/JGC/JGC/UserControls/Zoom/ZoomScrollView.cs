using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.UserControls.Zoom
{
    public class ZoomScrollView : ScrollView
    {
        public static readonly BindableProperty MaximumZoomProperty = BindableProperty.Create(nameof(MaximumZoom), typeof(double), typeof(ZoomScrollView), 1d);

        public double MaximumZoom
        {
            get { return (double)GetValue(MaximumZoomProperty); }
            set { SetValue(MaximumZoomProperty, value); }
        }

        public static readonly BindableProperty MinimumZoomProperty = BindableProperty.Create(nameof(MinimumZoom), typeof(double), typeof(ZoomScrollView), 1d);

        public double MinimumZoom
        {
            get { return (double)GetValue(MinimumZoomProperty); }
            set { SetValue(MinimumZoomProperty, value); }
        }
    }
}
