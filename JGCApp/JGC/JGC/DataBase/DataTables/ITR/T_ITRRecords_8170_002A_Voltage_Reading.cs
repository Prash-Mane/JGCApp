using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8170_002A_Voltage_Reading")]
    public class T_ITRRecords_8170_002A_Voltage_Reading
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string L1_L2 { get; set; }
        public string L2_L3 { get; set; }
        public string L3_L1 { get; set; }
        public string L1_N { get; set; }
        public string L2_N { get; set; }
        public string L3_N { get; set; }
        public string PhaseRotation { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqL1_L2 { get; set; }
        [Ignore]
        public bool IsReqL2_L3 { get; set; }
        [Ignore]
        public bool IsReqL3_L1 { get; set; }
        [Ignore]
        public bool IsReqL1_N { get; set; }
        [Ignore]
        public bool IsReqL2_N { get; set; }
        [Ignore]
        public bool IsReqL3_N { get; set; }
        [Ignore]
        public bool IsReqPhaseRotation { get; set; }
    }
}
