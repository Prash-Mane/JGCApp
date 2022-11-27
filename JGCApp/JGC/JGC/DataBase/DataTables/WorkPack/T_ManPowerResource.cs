using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_ManPowerResource")]
    public class T_ManPowerResource
    {
        public string WorkerID { get; set; }
        public string WorkerName { get; set; }
        public string CompanyCode { get; set; }
        public string SectionCode { get; set; }
        public string FunctionCode { get; set; }
        public string RFID { get; set; }
        public string BeaconID { get; set; }
    }
}
