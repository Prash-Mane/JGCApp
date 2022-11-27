using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8161_002A_Body")]
    public class T_ITR8161_002A_Body
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string TestVolt { get; set; }
        public string TestVoltDuration { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public DateTime? CalibrationDate1 { get; set; }
        public string Instrument2 { get; set; }
        public string InstrumentSerialNo2 { get; set; }
        public DateTime? CalibrationDate2 { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqTestVolt { get; set; }
        [Ignore]
        public bool IsReqTestVoltDuration { get; set; }
    }
}
