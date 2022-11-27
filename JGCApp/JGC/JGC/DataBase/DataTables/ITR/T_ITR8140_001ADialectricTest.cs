using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8140_001ADialectricTest")]
    public class T_ITR8140_001ADialectricTest
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }    
        public long ITRCommonID { get; set; }
        public string TestPhase { get; set; }
        public string TestVoltage1 { get; set; }
        public string InsulationResistance1 { get; set; }
        public string AppliedPoint { get; set; }
        public string TestVoltageAndDuration { get; set; }
        public string ChargeCurrent { get; set; }
        public string TestVoltage2 { get; set; }
        public string InsulationResistance2 { get; set; }
        public string ResultsPassOrFail { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqTestPhase { get; set; }
        [Ignore]
        public bool IsReqTestVoltage1 { get; set; }
        [Ignore]
        public bool IsReqInsulationResistance1 { get; set; }
        [Ignore]
        public bool IsReqAppliedPoint { get; set; }
        [Ignore]
        public bool IsReqTestVoltageAndDuration { get; set; }
        [Ignore]
        public bool IsReqChargeCurrent { get; set; }
        [Ignore]
        public bool IsReqTestVoltage2 { get; set; }
        [Ignore]
        public bool IsReqInsulationResistance2 { get; set; }
    }
}
