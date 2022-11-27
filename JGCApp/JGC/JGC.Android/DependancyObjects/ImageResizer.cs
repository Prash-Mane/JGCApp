using System;
using System.IO;
using Android.Graphics;
using JGC.Common.Interfaces;
using JGC.Droid.DependancyObjects;

[assembly: Xamarin.Forms.Dependency(typeof(ImageResizer))]
namespace JGC.Droid.DependancyObjects
{
    public class ImageResizer : IImageResizer
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            float rHeight = 0;
            float rWidth = 0;
            var oriHeight = originalImage.Height;
            var oriWidth = originalImage.Width;

            if (oriHeight > oriWidth)
            {
                rHeight = height;
                float ratio = oriHeight / height;
                rWidth = oriWidth / ratio;
            }
            else
            {
                rWidth = width;
                float ratio = oriWidth / width;
                rHeight = oriHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)rWidth, (int)rHeight, false);
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}