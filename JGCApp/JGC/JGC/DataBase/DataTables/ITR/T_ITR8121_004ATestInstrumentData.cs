using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8121_004ATestInstrumentData")]
    public class T_ITR8121_004ATestInstrumentData
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string HvToEarth { get; set; }
        public string LvToEarth { get; set; }
        public string HvToLv { get; set; }
        public string TestVoltage { get; set; }
        public string Instrument { get; set; }
        public string InstrumentSerialNo { get; set; }
        public DateTime? CalibrationDate { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public DateTime? CalibrationDate1 { get; set; }
        public string Instrument2 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public DateTime? CalibrationDate2 { get; set; }
        public string AmbientTemperature { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqHvToEarth { get; set; }
        [Ignore]
        public bool IsReqLvToEarth { get; set; }
        [Ignore]
        public bool IsReqHvToLv { get; set; }
        [Ignore]
        public bool IsReqTestVoltage { get; set; }
        [Ignore]
        public bool IsReqInstrument { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo { get; set; }
        [Ignore]
        public bool IsReqInstrument1 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo1 { get; set; }
        [Ignore]
        public bool IsReqInstrument2 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo2 { get; set; }
        [Ignore]
        public bool IsReqAmbientTemperature { get; set; }
    }
}
