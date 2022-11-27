using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{

    [Table("T_ITRRecords_8000_003A")]
    public class T_ITR8000_003ARecords
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public long ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string VOLTAGE { get; set; }
        public string CableTagNo { get; set; }
        public string TYPEAndSIZE { get; set; }
        public string Instrument { get; set; }
        public string InstrumentSerialNo { get; set; }
        public DateTime? CalibrationDate { get; set; }
        public string CableName { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqInstrument { get; set; }
        [Ignore]
        public bool IsReqInstrumentSerialNo { get; set; }
        [Ignore]
        public bool IsReqVOLTAGE { get; set; }
        [Ignore]
        public bool IsReqTYPEAndSIZE { get; set; }
    }
}
