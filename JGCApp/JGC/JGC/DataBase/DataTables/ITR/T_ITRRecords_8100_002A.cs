using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8100_002A")]
    public class T_ITRRecords_8100_002A
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string Ratio { get; set; }
        public string ClassVA { get; set; }
        public string RatedVolt { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public DateTime CalibrationDate1 { get; set; }
        public string Instrument2 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public DateTime CalibrationDate2 { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqModelNo { get; set; }
        [Ignore]
        public bool IsReqSerialNo { get; set; }
        [Ignore]
        public bool IsReqRatio { get; set; }
        [Ignore]
        public bool IsReqClassVA { get; set; }
        [Ignore]
        public bool IsReqRatedVolt { get; set; }
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
