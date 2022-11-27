using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_TagMilestoneStatus")]
    public class T_TagMilestoneStatus
    {
        public override string ToString()
        {
            return TagNo;
        }
        public int Project_ID { get; set; }
        public int IWPID { get; set; }
        public int CWPTagID { get; set; }
        public string TagNo { get; set; }
        public string Milestone { get; set; }
        public string MilestoneAttribute { get; set; }
        //public string SignedByUserID { get; set; }
        public string UpdatedByUserID { get; set; }
        //  public string SignedBy { get; set; }
        public bool SignedInCMS { get; set; }
        public bool SignedInVM { get; set; }
        public DateTime? StatusValue { get; set; }
        public string TagMember { get; set; }
        public bool Updated { get; set; }

    }
}
