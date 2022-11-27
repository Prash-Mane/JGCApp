using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8100_002A_Radio_Test")]
    public class T_ITRRecords_8100_002A_Radio_Test
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string PrimaryVoltage { get; set; }
        public string L1N { get; set; }
        public string L2N { get; set; }
        public string L3N { get; set; }
        public string L1L2 { get; set; }
        public string L2L3 { get; set; }
        public string L3L1 { get; set; }
        public string Result { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public string InsHeading { get; set; }
        [Ignore]
        public bool IsEnabledL1N { get; set; }
        [Ignore]
        public bool IsEnabledL2N { get; set; }
        [Ignore]
        public bool IsEnabledL3N { get; set; }
        [Ignore]
        public bool IsEnabledL1L2 { get; set; }
        [Ignore]
        public bool IsEnabledL2L3 { get; set; }
        [Ignore]
        public bool IsEnabledL3L1 { get; set; }
        [Ignore]
        public string IsEnabledL1NBG { get; set; }
        [Ignore]
        public string IsEnabledL2NBG { get; set; }
        [Ignore]
        public string IsEnabledL3NBG { get; set; }
        [Ignore]
        public string IsEnabledL1L2BG { get; set; }
        [Ignore]
        public string IsEnabledL2L3BG { get; set; }
        [Ignore]
        public string IsEnabledL3L1BG { get; set; }

        [Ignore]
        public bool IsReqPrimaryVoltage { get; set; }
        [Ignore]
        public bool IsReqL1N { get; set; }
        [Ignore]
        public bool IsReqL2N { get; set; }
        [Ignore]
        public bool IsReqL3N { get; set; }
        [Ignore]
        public bool IsReqL1L2 { get; set; }
        [Ignore]
        public bool IsReqL2L3 { get; set; }
        [Ignore]
        public bool IsReqL3L1 { get; set; }
        [Ignore]
        public bool IsReqResult { get; set; }
    }
}
