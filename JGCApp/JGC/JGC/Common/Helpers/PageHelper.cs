using JGC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
    public class PageHelper 
    {
        public int GridViewRowIndex { get; set; }
        public int EReportID { get; set; }
        public string Token { get; set; }
        public string TokenTimeStamp { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string CameraDirectory { get; set; }
        public string CameraStatusValue { get; set; }
        public string ReportType { get; set; }
        public string UnitID { get; set; }
        public string Error { get; set; }

    }
}
