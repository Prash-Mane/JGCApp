using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8100_001A_RatioTest")]
    public class T_ITR8100_001A_RatioTest
    {
        [Ignore]
        public int RowNo { get; set; }
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long CommonHFID { get; set; }
        public string Phase { get; set; }
        public string PrimaryCurrent { get; set; }
        public string SecondaryCurrent { get; set; }
        public string CalculatedCTRatio { get; set; }
        public string AmmeterReading { get; set; }
        public string ITRNumber { get; set; }
        public string TagNO { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }

        [Ignore]
        public bool IsReqPhase { get; set; }
        [Ignore]
        public bool IsReqPrimaryCurrent { get; set; }
        [Ignore]
        public bool IsReqSecondaryCurrent { get; set; }
        [Ignore]
        public bool IsReqCalculatedCTRatio { get; set; }
        [Ignore]
        public bool IsReqAmmeterReading { get; set; }
    }
}
