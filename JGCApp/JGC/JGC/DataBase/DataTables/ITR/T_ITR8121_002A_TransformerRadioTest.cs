using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8121_002A_TransformerRadioTest")]
    public class T_ITR8121_002A_TransformerRadioTest
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public string TapNo { get; set; }
        public string TapVoltagePrimary { get; set; }
        public string TapVoltageSecondary { get; set; }
        public string CalculatedRatio { get; set; }
        public string TestValueL1Ratio { get; set; }
        public string TestValueL1Error { get; set; }
        public int ID_8121_002A_TransformerRadioTest { get; set; }
        public string TestValueL2Ratio { get; set; }
        public string TestValueL2Error { get; set; }
        public string TestValueL3Ratio { get; set; }
        public string TestValueL3Error { get; set; }
        public string result { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }

        [Ignore]
        public bool IsReqTapNo { get; set; }
        [Ignore]
        public bool IsReqTapVoltagePrimary { get; set; }
        [Ignore]
        public bool IsReqTapVoltageSecondary { get; set; }
        [Ignore]
        public bool IsReqCalculatedRatio { get; set; }
        [Ignore]
        public bool IsReqTestValueL1Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL1Error { get; set; }
        //public int ID_8121_002A_TransformerRadioTest { get; set; }
        [Ignore]
        public bool IsReqTestValueL2Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL2Error { get; set; }
        [Ignore]
        public bool IsReqTestValueL3Ratio { get; set; }
        [Ignore]
        public bool IsReqTestValueL3Error { get; set; }
        [Ignore]
        public bool IsReqresult { get; set; }
    }
}
