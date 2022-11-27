using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPAdminControlLog")]
    public class T_IWPAdminControlLog
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string SignatureName { get; set; }
        public int SignatureNo { get; set; }
        public string FunctionCode { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string SectionCode { get; set; }
        public string DatahubField { get; set; }
        public bool MilestoneReturnAfterRevision { get; set; }
        public bool ExecutionStart { get; set; }
        public bool ExecutionFinish { get; set; }
        public bool Validate { get; set; }
        public bool PunchesCompleted { get; set; }
        public bool PunchesConfirmed { get; set; }
        [Ignore]
        public List<string> PunchCategory { get; set; }
        [Ignore]
        public List<string> PunchLayer { get; set; }
    }
}
