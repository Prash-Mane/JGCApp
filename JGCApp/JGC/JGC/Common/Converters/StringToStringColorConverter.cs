using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class StringToStringColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            var GivenString = (string)value;
            string ColorAsString = "";
            try
            {
                string last_char = string.Empty;



                if (GivenString.ToUpper() != "CLOSED" && GivenString.ToUpper() != "OPEN" && GivenString.ToUpper() != "CANCELLED")
                    last_char = GivenString[GivenString.Length - 1].ToString();
                else
                    last_char = GivenString;

               switch (last_char.ToUpper())
                {
                    case "A":
                        ColorAsString = "#ff0000";
                        break;
                    case "B":
                        ColorAsString = "#0000ff";
                        break;
                    case "C":
                        ColorAsString = "#00ff00";
                        break;
                    case "CLOSED":
                         ColorAsString = "Red";
                        break;
                    case "OPEN":
                         ColorAsString = "Green";
                        break;
                    case "CANCELLED":
                        ColorAsString = "Silver";
                        break;
                }

                return ColorAsString;
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

