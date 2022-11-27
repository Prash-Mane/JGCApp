using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8140_001ATestInstrucitonData")]
    public class T_ITR8140_001ATestInstrucitonData
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long  ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public DateTime? CalibrationDate1 { get; set; }
        public string Instrument2 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public DateTime? CalibrationDate2 { get; set; }
        public string Instrument3 { get; set; }
        public string InstrumentSerialNo3 { get; set; }
        public DateTime? CalibrationDate3 { get; set; }
        public string AmbientTemperature { get; set; }
        public string AppliedTestVoltage { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        public string AfiNo { get; set; }
       
        [Ignore]
        public bool IsReqInstrument1 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo1 { get; set; }
        [Ignore]
        public bool IsReqInstrument2 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo2 { get; set; }
        [Ignore]
        public bool IsReqInstrument3 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo3 { get; set; }
        [Ignore]
        public bool IsReqAmbientTemperature { get; set; }
        [Ignore]
        public bool IsReqAppliedTestVoltage { get; set; }
    }
}
