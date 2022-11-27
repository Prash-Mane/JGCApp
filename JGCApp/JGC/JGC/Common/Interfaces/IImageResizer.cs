using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Interfaces
{
    public interface IImageResizer
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
