using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class ZeroToEmptyEntryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float v = float.Parse(value.ToString());
            if (!(v > 0))
                return string.Empty;
            return v.ToString("F3");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(string.IsNullOrWhiteSpace(value.ToString()))
                return 0;
            else
            {
                float v = float.Parse(value.ToString());
                return  float.Parse(v.ToString("F3"));
            }
        }
    }
}
