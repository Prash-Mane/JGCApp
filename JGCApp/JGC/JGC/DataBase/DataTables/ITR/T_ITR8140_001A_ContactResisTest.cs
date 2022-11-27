using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8140_001A_ContactResisTest")]
    public class T_ITR8140_001A_ContactResisTest
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public long ITRCommonID { get; set; }
        public int ID { get; set; }
        public string ConnectionFrom { get; set; }
        public string ConnectionTo { get; set; }
        public bool TorqueMarkOk { get; set; }
        public string ContactResMeasuredValL1 { get; set; }
        public string ContactResMeasuredValL2 { get; set; }
        public string ContactResMeasuredValL3 { get; set; }
        public string ContactResMeasuredValN { get; set; }
        public string ContactResMeasuredValL5 { get; set; }
        public string Remarks { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        public string TorqueMarkOkValue { get; set; }
        public long row_Id { get; set; }
        [Ignore]
        public bool IsReqConnectionFrom { get; set; }
        [Ignore]
        public bool IsReqConnectionTo { get; set; }
        [Ignore]
        public bool IsReqTorqueMarkOk { get; set; }
        [Ignore]
        public bool IsReqContactResMeasuredValL1 { get; set; }
        [Ignore]
        public bool IsReqContactResMeasuredValL2 { get; set; }
        [Ignore]
        public bool IsReqContactResMeasuredValL3 { get; set; }
        [Ignore]
        public bool IsReqContactResMeasuredValN { get; set; }
        [Ignore]
        public bool IsReqContactResMeasuredValL5 { get; set; }
    }
}
