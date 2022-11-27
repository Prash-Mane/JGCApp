using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IITRService
    {
        Task<bool> IsImplementedITR(string SheetName);
        Task<bool> ITR_3XA(string SheetName);
    }
}
