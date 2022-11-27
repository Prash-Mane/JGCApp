using JGC.Common.Interfaces;
using JGC.UWP.DependancyObjects;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Xamarin.Forms;
using System.Runtime.InteropServices.WindowsRuntime;



[assembly: Dependency(typeof(ImageResizer))]
namespace JGC.UWP.DependancyObjects
{
    public class ImageResizer : IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            return Task.Run(async () => await getResizeImage(imageData, width, height)).Result;
        }
        public async Task<byte[]> getResizeImage(byte[] imageData, float width, float height)
        {
            byte[] resizedData;

            using (var streamIn = new MemoryStream(imageData))
            {
                using (var imageStream = streamIn.AsRandomAccessStream())
                {
                    var decoder = await BitmapDecoder.CreateAsync(imageStream);
                    var resizedStream = new InMemoryRandomAccessStream();
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;
                    encoder.BitmapTransform.ScaledHeight = (uint)height;
                    encoder.BitmapTransform.ScaledWidth = (uint)width;
                    await encoder.FlushAsync();
                    resizedStream.Seek(0);
                    resizedData = new byte[resizedStream.Size];
                    await resizedStream.ReadAsync(resizedData.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);
                }
            }
            return resizedData;
        }
    }
}
