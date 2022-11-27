using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8100_002A_InsRes_Test")]
    public class T_ITRRecords_8100_002A_InsRes_Test
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string WindingL1 { get; set; }
        public string WindingL2 { get; set; }
        public string WindingL3 { get; set; }
        public string WindingResult { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public string InsHeading { get; set; }
        [Ignore]
        public bool IsReqWindingL1 { get; set; }
        [Ignore]
        public bool IsReqWindingL2 { get; set; }
        [Ignore]
        public bool IsReqWindingL3 { get; set; }
        [Ignore]
        public bool IsReqWindingResult { get; set; }
    }
}
