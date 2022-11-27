using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8100_001A_TestInstrumentData")]
    public class T_ITR8100_001A_TestInstrumentData
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long CommonHFID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string Instrument { get; set; }
        public string InstrumentSerialNo { get; set; }
        public DateTime CalibrationDate { get; set; }
        public string ITRNumber { get; set; }
        public string TagNO { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqInstrument { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo { get; set; }

    }
}
