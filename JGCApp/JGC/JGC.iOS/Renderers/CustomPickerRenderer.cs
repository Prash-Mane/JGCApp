using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using JGC.iOS.Renderers;
using JGC.UserControls.CustomControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace JGC.iOS.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            SetControlStyle();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetControlStyle();
        }

        private void SetControlStyle()
        {
            if (Control != null)
            {
                var imgDropDownArrow = UIImage.FromFile("ddarrow.png");
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIImageView(imgDropDownArrow);
            }
        }


    }
}