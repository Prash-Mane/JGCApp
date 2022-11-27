using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_PunchList")]
    public class T_PunchList
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int ETestPackageID { get; set; }
        public string TestPackage { get; set; }
        public int VMHub_DocumentsID { get; set; }
        public int PunchAdminID { get; set; }
        public string PunchID { get; set; }
        public int PunchNo { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string FunctionCode { get; set; }
        public string CompanyCategoryCode { get; set; }
        public bool OnDocument { get; set; }
        public int XPOS1 { get; set; }
        public int YPOS1 { get; set; }
        public int XPOS2 { get; set; }
        public int YPOS2 { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool TPCConfirmed { get; set; }
        public int TPCConfirmedByUserID { get; set; }
        public string TPCConfirmedBy { get; set; }
        public DateTime TPCConfirmedOn { get; set; }
        public bool WorkCompleted { get; set; }
        public int WorkCompletedByUserID { get; set; }
        public string WorkCompletedBy { get; set; }
        public DateTime WorkCompletedOn { get; set; }
        public bool WorkConfirmed { get; set; }
        public int WorkConfirmedByUserID { get; set; }
        public string WorkConfirmedBy { get; set; }
        public DateTime WorkConfirmedOn { get; set; }
        public bool Cancelled { get; set; }
        public int CancelledByUserID { get; set; }
        public string CancelledBy { get; set; }
        public DateTime CancelledOn { get; set; }
        public string Status { get; set; }
        public string DisciplineCode { get; set; }
        public string IssuerCode { get; set; }
        public string SystemNo { get; set; }
        public string SpoolDrawingNo { get; set; }
        public string PCWBS { get; set; }
        public bool Live { get; set; }
        public bool Updated { get; set; }
        public bool WorkRejected { get; set; }
        public int WorkRejectedByUserID { get; set; }
        public DateTime WorkRejectedOn { get; set; }
        public string WorkRejectedReason { get; set; }
        public string WorkRejectedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string PresetPunchID { get; set; }
        public string WorkRejectedCompletedBy { get; set; }
        public DateTime WorkRejectedCompletedOn { get; set; }




    }
}
