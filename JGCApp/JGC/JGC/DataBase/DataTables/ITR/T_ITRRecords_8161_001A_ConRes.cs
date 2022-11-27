using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8161_001A_ConRes")]
    public class T_ITRRecords_8161_001A_ConRes
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        [Ignore]
        public int SlNo { get; set; }
        public string ConnoFrom { get; set; }
        public string ConnoTo { get; set; }
        public string CRMVL1 { get; set; }
        public string CRMVL2 { get; set; }
        public string CRMVL3 { get; set; }
        public string CRMVN { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqConnoFrom { get; set; }
        [Ignore]
        public bool IsReqConnoTo { get; set; }
        [Ignore]
        public bool IsReqCRMVL1 { get; set; }
        [Ignore]
        public bool IsReqCRMVL2 { get; set; }
        [Ignore]
        public bool IsReqCRMVL3 { get; set; }
    }
}
