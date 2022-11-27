using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPPunchControlItem")]
    public class T_IWPPunchControlItem
    {
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int IWPID { get; set; }
        public int CWPID { get; set; }
        public int VMHub_DocumentsID { get; set; }
        public int PunchAdminID { get; set; }
        public string PunchID { get; set; }
        public int PunchNo { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string FunctionCode { get; set; }
        public string CompanyCategoryCode { get; set; }
        public Boolean OnDocument { get; set; }
        public int XPOS1 { get; set; }
        public int YPOS1 { get; set; }
        public int XPOS2 { get; set; }
        public int YPOS2 { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime TargetDate { get; set; }
        
        public Boolean WorkCompleted { get; set; }
        public int WorkCompletedByUserID { get; set; }
        public string WorkCompletedBy { get; set; }
        public DateTime WorkCompletedOn { get; set; }

        public Boolean WorkConfirmed { get; set; }
        public int WorkConfirmedByUserID { get; set; }
        public string WorkConfirmedBy { get; set; }
        public DateTime WorkConfirmedOn { get; set; }

        public Boolean WorkRejected { get; set; }
        public int WorkRejectedByUserID { get; set; }
        public string WorkRejectedBy { get; set; }
        public DateTime WorkRejectedOn { get; set; }
        public string WorkRejectedReason { get; set; }

        public Boolean Cancelled { get; set; }
        public int CancelledByUserID { get; set; }
        public string CancelledBy { get; set; }
        public DateTime CancelledOn { get; set; }
        public string Status { get; set; }
        public string DrawingRevision { get; set; }
        public bool Updated { get; set; }
        public int UpdatedByUserID { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsCreated { get; set; }
        public string SectionCode { get; set; }
        public string OtherComponent { get; set; }
        public string PunchType { get; set; }
        public string Component { get; set; }
        public string ComponentCategory { get; set; }
        public string Action { get; set; }
     
    }
}
