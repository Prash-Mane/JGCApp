using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8170_002A_InsRes")]
    public class T_ITR_8170_002A_InsRes
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string AllLineToEarth { get; set; } 
        public string OnOffOperation { get; set; }
        public string Instrument { get; set; }
        public string InstrumentSerialNo { get; set; }
        public Nullable<DateTime> CalibrationDate { get; set; }
        public string Description { get; set; }
        public string AmbientTemp { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqAllLineToEarth { get; set; }
        [Ignore]
        public bool IsReqAmbientTemp { get; set; }
        [Ignore]
        public bool IsReqOnOffOperation { get; set; }
    }
}
