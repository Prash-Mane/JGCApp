using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8300_003A_Body")]
   public class T_ITR_8300_003A_Body
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string Instrument1 { get; set; }
        public string InstrumentSerialNo1 { get; set; }
        public string InstrumentDesc1 { get; set; }
        public string CalibrationDate1 { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
    }
}
