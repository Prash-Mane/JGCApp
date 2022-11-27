using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_30A_31A")]
    public class T_ITRRecords_30A_31A 
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public long ID { get; set; }
        public long CommonHFID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public DateTime DateToday { get; set; }
        public string CableLength { get; set; }
        public string CableLengthUnit { get; set; }
        public string OTDRModel { get; set; }
        public string SpliceQty { get; set; }
        public string FiberInformation { get; set; }
        public DateTime CalibrationDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ITRNumber { get; set; }
        public string TagNO { get; set; }
        public string FiberinformationUnit { get; set; }
        public string AfiNo { get; set; }
        public string TestResultsAccepted { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqOTDRModel { get; set; }
        [Ignore]
        public bool IsReqSpliceQty { get; set; }
        [Ignore]
        public bool IsReqCableLength { get; set; }
        [Ignore]
        public bool IsReqCableLengthUnit { get; set; }
        [Ignore]
        public bool IsReqFiberInformation { get; set; }
        [Ignore]
        public bool IsReqFiberinformationUnit { get; set; }
    }
}
