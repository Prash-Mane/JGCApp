using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8121_002A_Records")]
    public class T_ITR8121_002A_Records
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public string HVtoEarth { get; set; }
        public string LVtoEarth { get; set; }
        public string HVtoLV { get; set; }
        public string TestVoltage { get; set; }
        public string OilTemp { get; set; }
        public string Instrument1 { get; set; }
        public string Instrument2 { get; set; }
        public string Instrument3 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public string InstrumentSerialNo3 { get; set; }
        public DateTime CalibrationDate1 { get; set; }
        public DateTime CalibrationDate2 { get; set; }
        public DateTime CalibrationDate3 { get; set; }
        public long ITRCommonID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string DialeticStrengthTest1 { get; set; }
        public string DialeticStrengthTest2 { get; set; }
        public string DialeticStrengthTest3 { get; set; }
        public string DialeticStrengthTest4 { get; set; }
        public string DialeticStrengthTest5 { get; set; }
        public string DialeticStrengthTest6 { get; set; }
        public string DialeticStrengthTestAverage { get; set; }
        public string Result { get; set; }
        public string AmbientTemperature { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }

        [Ignore]
        public bool IsReqHVtoEarth { get; set; }
        [Ignore]
        public bool IsReqLVtoEarth { get; set; }
        [Ignore]
        public bool IsReqHVtoLV { get; set; }
        [Ignore]
        public bool IsReqTestVoltage { get; set; }
        [Ignore]
        public bool IsReqOilTemp { get; set; }

        [Ignore]
        public bool IsReqInstrument1 { get; set; }
        [Ignore]
        public bool IsReqInstrument2 { get; set; }
        [Ignore]
        public bool IsReqInstrument3 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo1 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo2 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo3 { get; set; }

        [Ignore]
        public bool IsReqDialeticStrengthTest1 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTest2 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTest3 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTest4 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTest5 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTest6 { get; set; }
        [Ignore]
        public bool IsReqDialeticStrengthTestAverage { get; set; }
        [Ignore]
        public bool IsReqResult { get; set; }
        [Ignore]
        public bool IsReqAmbientTemperature { get; set; }
    }
}
