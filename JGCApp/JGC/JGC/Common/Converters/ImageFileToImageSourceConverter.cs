using JGC.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Converters
{
    public class ImageFileToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var path = (string)value;

                var _file = Task.Run(async () => await DependencyService.Get<ISaveFiles>().GetThumbnail(path));
                //Stream stream = new MemoryStream(_file.Result);
                return _file.Result;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private async Task<ImageSource> getfile(string path)
        {
            return await DependencyService.Get<ISaveFiles>().GetThumbnail(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
