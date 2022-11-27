using SQLite;
using System;

namespace JGC.DataBase.DataTables
{
    
    [Table("T_PostTestRecordAcceptedBy")]
    public class T_PostTestRecordAcceptedBy
    {
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public int AdminID { get; set; }
        public bool Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
        public bool Live { get; set; }
        public bool Updated { get; set; }
        public string Error { get; set; }
    }
}
