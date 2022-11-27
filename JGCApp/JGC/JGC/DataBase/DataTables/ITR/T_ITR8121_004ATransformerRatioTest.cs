using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8121_004ATransformerRatioTest")]
    public class T_ITR8121_004ATransformerRatioTest
    {
        [PrimaryKey]

        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string TapNo { get; set; }
        public string TapVoltagePrimary { get; set; }
        public string TapVoltageSecondary { get; set; }
        public string CalculateRatio { get; set; }
        public string TestValueL1Ratio { get; set; }
        public string TestValueL1Error { get; set; }
        public string TestValueL2Ratio { get; set; }
        public string TestValueL2Error { get; set; }
        public string TestValueL3Ratio { get; set; }
        public string TestValueL3Error { get; set; }
        public string Result { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqTapNo { get; set; }
        [Ignore]
        public bool IsReqTapVoltagePrimary { get; set; }
        [Ignore]
        public bool IsReqTapVoltageSecondary { get; set; }
        [Ignore]
        public bool IsReqCalculateRatio { get; set; }
        [Ignore]
        public bool IsReqTestValueL1Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL1Error { get; set; }
        [Ignore]
        public bool IsReqTestValueL2Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL2Error { get; set; }
        [Ignore]
        public bool IsReqTestValueL3Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL3Error { get; set; }
        [Ignore]
        public bool IsReqResult { get; set; }
    }
}
