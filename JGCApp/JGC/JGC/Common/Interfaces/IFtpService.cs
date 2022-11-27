using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Interfaces
{
    public interface IFtpService
    {
        List<dynamic> DownloadFiles(string id);
        List<string> FtpFileNames(string id);
    }
}
