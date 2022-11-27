using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class DateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ColorAsString = "Black";
            if (value == null) // || value.ToString().ToLower() == "false"
                return ColorAsString;
            DateTime GivenDate = (DateTime)value;
           
            try
            {
                if (GivenDate > DateTime.Now)
                    ColorAsString = "#FB1610";
                return ColorAsString;
            }
            catch
            {
                return ColorAsString;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
