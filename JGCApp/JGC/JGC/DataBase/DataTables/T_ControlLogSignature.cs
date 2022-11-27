using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_ControlLogSignature")]
    public class T_ControlLogSignature
    {
        //[PrimaryKey, Indexed]
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public int ControlLogAdminID { get; set; }
        public bool Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
        public bool Live { get; set; }
        public bool Updated { get; set; }
        public bool Reject { get; set; }
        public int RejectedByUserID { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectComment { get; set; }
        public string Error { get; set; }
    }
}
