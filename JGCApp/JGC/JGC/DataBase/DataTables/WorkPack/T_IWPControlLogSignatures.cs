using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPControlLogSignatures")]
    public class T_IWPControlLogSignatures
    {
        public string Error { get; set; }
        public int ControlLogAdminID { get; set; }
        public int ProjectID { get; set; }
        public int IWP_ID { get; set; }
        public bool Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
        public bool Reject { get; set; }
        public int RejectedByUserID { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectComment { get; set; }
        public bool Updated { get; set; }
    }
}
