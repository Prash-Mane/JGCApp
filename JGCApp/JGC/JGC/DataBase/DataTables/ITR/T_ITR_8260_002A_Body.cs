using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8260_002A_Body")]
    public class T_ITR_8260_002A_Body
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string TestVolt { get; set; }
        public string CableType { get; set; }
        public string OperationVolt { get; set; }
        public string RatedVolt { get; set; }
        public string TestVoltDuration { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public DateTime CalibrationDate1 { get; set; }
        public string Instrument2 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public DateTime CalibrationDate2 { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }

        [Ignore]
        public bool IsReqTestVolt { get; set; }
        [Ignore]
        public bool IsReqCableType { get; set; }
        [Ignore]
        public bool IsReqOperationVolt { get; set; }
        [Ignore]
        public bool IsReqRatedVolt { get; set; }
        [Ignore]
        public bool IsReqTestVoltDuration { get; set; }
        [Ignore]
        public bool IsReqInstrument1 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo1 { get; set; }
        [Ignore]
        public bool IsReqInstrument2 { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo2 { get; set; }
    }
}
