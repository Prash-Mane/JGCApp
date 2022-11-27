using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPStatus")]
    public class T_IWPStatus
    {
        public int Activity_ID { get; set; }
        public int IWP_ID { get; set; }
        public int ActivityNo { get; set; }
        public string Activity { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Error { get; set; }
    }
}
