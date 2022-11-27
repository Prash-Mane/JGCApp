using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace JGC.UserControls.CustomControls
{
    public class CustomPDF : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
                returnType: typeof(string),
                declaringType: typeof(CustomPDF),
                defaultValue: default(string));

        //public static readonly BindableProperty UriProperty = BindableProperty.Create<CustomPDF, string>(p => p.Uri, default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value);
                OnPropertyChanged("Uri");
            }
        }
    }
}
