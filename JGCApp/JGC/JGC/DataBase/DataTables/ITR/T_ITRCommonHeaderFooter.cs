using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRCommonHeaderFooter")]
    public class T_ITRCommonHeaderFooter
    {
        [PrimaryKey]
        public long ROWID { get; set; }
        public long ID { get; set; }
        public string ITRNumber { get; set; }
        public string ITRDescription { get; set; }
        public string ITRRev { get; set; }
        public int ReportID { get; set; }
        public string ReportNo { get; set; }
        public string Project { get; set; }
        public string JobCode { get; set; }
        public string System { get; set; }
        public string Tag { get; set; }
        public string SubSystem { get; set; }
        public string TagDescription { get; set; }
        public string PCWBS { get; set; }
        public string DrawingNo { get; set; }
        public string FWBS { get; set; }
        public string DrawingRev { get; set; }
        public string Remarks { get; set; }
        public string SignSubcontractor { get; set; }
        public DateTime SignSubcontractorDate { get; set; }
        public string SignContractor { get; set; }
        public DateTime SignContractorDate { get; set; }
        public string Client { get; set; }
        public DateTime ClientDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string AFINo { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        public bool IsCreatedHH { get; set; }
        public bool Started { get; set; }
        public string StartedBy { get; set; }
        public DateTime StartedOn { get; set; }
        public bool Completed { get; set; }
        public string Completedby { get; set; }
        public DateTime Completedon { get; set; }
        public bool Rejected { get; set; }
        public string RejectedReason { get; set; }
        public DateTime rejectedDate { get; set; }
        public int RejectedUserID { get; set; }
        [Ignore]
        public List<T_CommonHeaderFooterSignOff> CommonHeaderFooterSignOff { get; set; }
        [Ignore]
        public List<T_ITRInstrumentData> ITRInstrumentData { get; set; }
        [Ignore]
        public string StatusColor { get; set; }
        [Ignore]
        public bool IsDrowingTextShow { get; set; }
        [Ignore]
        public bool IsDrowingDropDownShow { get; set; }
        [Ignore]
        public string SelectedDrawingNo { get; set; }

    }
}
