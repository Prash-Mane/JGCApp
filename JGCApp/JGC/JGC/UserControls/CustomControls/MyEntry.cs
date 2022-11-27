
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC
{
   
    public class MyEntry : Entry
    {
        public static readonly BindableProperty NeedToFocusProperty = BindableProperty.Create(nameof(NeedToFocus), typeof(bool), typeof(MyEntry), default(bool), propertyChanged: OnNeedToFocusChanged);
        public bool NeedToFocus
        {
            get { return (bool)GetValue(NeedToFocusProperty); }
            set { SetValue(NeedToFocusProperty, value); }
        }
        static void OnNeedToFocusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = (MyEntry)bindable;
            item.Focus();
        }
    }



    public class MyImage : Image
    {
        public static readonly BindableProperty NeedToFocusProperty = BindableProperty.Create(nameof(NeedToFocus), typeof(bool), typeof(MyImage), default(bool), propertyChanged: OnNeedToFocusChanged);
        public bool NeedToFocus
        {
            get { return (bool)GetValue(NeedToFocusProperty); }
            set { SetValue(NeedToFocusProperty, value); }
        }
        static void OnNeedToFocusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = (MyImage)bindable;
            item.Focus();
        }
    }
}
