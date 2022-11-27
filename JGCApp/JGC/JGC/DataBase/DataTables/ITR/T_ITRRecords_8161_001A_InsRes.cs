using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8161_001A_InsRes")]
    public class T_ITRRecords_8161_001A_InsRes
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public long ITRCommonID { get; set; }
        public string InsRes1 { get; set; }
        public string InsRes2 { get; set; }
        public string InsRes3 { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public string TestPhase1 { get; set; }
        [Ignore]
        public string TestPhase2 { get; set; }
        [Ignore]
        public string TestPhase3 { get; set; }
        [Ignore]
        public bool IsReqInsRes1 { get; set; }
        [Ignore]
        public bool IsReqInsRes2 { get; set; }
        [Ignore]
        public bool IsReqInsRes3 { get; set; }
    }
}
