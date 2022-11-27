using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var defaultColor = Color.FromRgba(0, 0, 0, 0);
            if (value == null) return defaultColor;
            var colorAsString = (string)value;
            try
            {
                return Color.FromHex(colorAsString);
            }
            catch
            {
                return defaultColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
