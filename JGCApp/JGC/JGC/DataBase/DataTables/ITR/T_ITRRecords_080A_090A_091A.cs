using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_080A_090A_091A")]
    public class T_ITRRecords_080A_090A_091A
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public long ID { get; set; }
        public string ITR { get; set; }
        public string Tag { get; set; }
        public string TestPressure { get; set; }
        public string TestPressureUom { get; set; }
        public bool TestResult { get; set; }
        public string AfiNo { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqTestPressure { get; set; }
        [Ignore]
        public bool IsReqTestPressureUom { get; set; }
    }
}
