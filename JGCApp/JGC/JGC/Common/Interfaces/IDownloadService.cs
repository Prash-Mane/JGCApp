using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IDownloadService
    {
        Task<bool> DownloadAsync(int selectedEReportIds);             
    }
}
