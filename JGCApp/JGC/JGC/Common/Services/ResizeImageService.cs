using JGC.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Services
{
    public class ResizeImageService : IResizeImageService
    {
        public async Task<byte[]> GetResizeImage(byte[] imageAsByte)
        {
            long bytesize = imageAsByte.Length;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Convert.ToDouble(bytesize);
            int order = 0;
            while (len >= 1024D && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            //string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", len, sizes[order]);
            if (order >= 2 && len > 1.0)
            {
                var imgResizer = DependencyService.Get<IImageResizer>();
                imageAsByte = imgResizer.ResizeImage(imageAsByte, 1024, 1024);
                return await GetResizeImage(imageAsByte);
            }
            else
                return imageAsByte;
        }
    }
}
