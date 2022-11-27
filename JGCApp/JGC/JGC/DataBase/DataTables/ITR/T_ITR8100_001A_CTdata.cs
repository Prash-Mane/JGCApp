using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8100_001A_CTdata")]
    public class T_ITR8100_001A_CTdata
    {
        [Ignore]
        public int RowNo { get; set; }
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long CommonHFID { get; set; }
        public string ModelNoTagNo { get; set; }
        public string SerialNo { get; set; }
        public string Ratio { get; set; }
        public string ClassVA { get; set; }
        public string ShortCircuitCurrent { get; set; }
        public string ITRNumber { get; set; }
        public string TagNO { get; set; }
        public string PrimaryCurrent { get; set; }
        public string SecondaryCurrent { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }

        [Ignore]
        public bool IsReqModelNoTagNo { get; set; }
        [Ignore]
        public bool IsReqSerialNo { get; set; }
        [Ignore]
        public bool IsReqRatio { get; set; }
        [Ignore]
        public bool IsReqClassVA { get; set; }
        [Ignore]
        public bool IsReqShortCircuitCurrent { get; set; }
        [Ignore]
        public bool IsReqPrimaryCurrent { get; set; }
        [Ignore]
        public bool IsReqSecondaryCurrent { get; set; }



    }
}
