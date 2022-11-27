using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_ManPowerLog")]
    public class T_ManPowerLog
    {
        public int ProjectID { get; set; }
        public int IWPID { get; set; }
        public string WPID { get; set; }
        public string WorkerID { get; set; }
        public string WorkerName { get; set; }
        public string CompanyCode { get; set; }
        public string SectionCode { get; set; }
        public string FunctionCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Updated { get; set; }
        public bool ISDeleted{ get; set; }
    }    
}
