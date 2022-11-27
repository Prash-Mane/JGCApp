using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.UserControls.CustomControls
{
    public class NestedScroll : Xamarin.Forms.ListView
    {
        public static readonly BindableProperty IsNestedScrollProperty = BindableProperty.Create(
            propertyName: "IsNestedScroll",
            returnType: typeof(bool),
            declaringType: typeof(NestedScroll),
            defaultValue: false
            );

        public bool IsNestedScroll
        {
            get
            {
                return (bool)GetValue(IsNestedScrollProperty);
            }
            set
            {
                SetValue(IsNestedScrollProperty, value);
            }
        }
    }
}
