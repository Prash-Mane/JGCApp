using JGC.UserControls.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.ColorChanged
{
    public class EmptyEntryValidatorBehavior : Behavior<ValidationEntry>
    {
        protected override void OnAttachedTo(ValidationEntry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }
        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) || !string.IsNullOrEmpty(e.OldTextValue))
            {
                ((ValidationEntry)sender).IsBorderErrorVisible = false;
            }
        }
        protected override void OnDetachingFrom(ValidationEntry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
        }
    }
}
