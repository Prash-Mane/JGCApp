using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        #region IValueConverter implementation
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var status = (string)value;
            if (status.Contains("Closed")) return "#FF0000";
            else return "Green";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
