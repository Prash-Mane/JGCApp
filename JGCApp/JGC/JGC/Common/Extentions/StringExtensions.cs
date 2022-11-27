using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace JGC.Common.Extentions
{
    public static class StringExtensions
    {
        public static string GetImage64String(this string input)
        {
            try
            {
                return input.Split(',')[1];
            }
            catch
            {
                return string.Empty;
            }
        }

        public static byte[] GetImageBytes(this string input)
        {
            var img64String = input.GetImage64String();
            if (string.IsNullOrEmpty(img64String))
                return null;
            return Convert.FromBase64String(img64String);
        }

        public static ImageSource GetImageSource(this string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    var imageBytes = input.GetImageBytes();
                    return ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DisplayAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() as DisplayAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string TruncateVMName(this string input) => input.Remove(input.Length - 9);
    }
}
