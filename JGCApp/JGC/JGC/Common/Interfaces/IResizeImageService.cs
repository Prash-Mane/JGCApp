using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IResizeImageService
    {
        Task<byte[]> GetResizeImage(byte[] imageAsByte);
    }
}
