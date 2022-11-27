using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class CMRHHStorageArea
    {
        public string Storage_Area { get; set; }
        public string JobCode { get; set; }
        public string Store_Location { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
        public double Avail_Stock_Qty { get; set; }
    }

    public class CMRHHHeatNos
    {
        public string Heat_No { get; set; }
        public string JobCode { get; set; }
        public string Store_Location { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
    }
}
