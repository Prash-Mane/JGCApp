using JGC.UserControls.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.ColorChanged
{
    public class EmptyRectanglePickerValidatorBehavior : Behavior<RectanglePicker>
    {
        protected override void OnAttachedTo(RectanglePicker bindable)
        {
            bindable.SelectedIndexChanged += bindable_SelectedIndexChanged;
        }
        void bindable_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (RectanglePicker)sender;
            if (!string.IsNullOrEmpty(picker.SelectedItem?.ToString()))
                ((RectanglePicker)sender).IsBorderErrorVisible = false;
        }
        protected override void OnDetachingFrom(RectanglePicker bindable)
        {
            bindable.SelectedIndexChanged -= bindable_SelectedIndexChanged;
        }
    }
}
