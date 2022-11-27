using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8100_001A_InsulationResistanceTest")]
    public class T_ITR8100_001A_InsulationResistanceTest
    {
        [Ignore]
        public int RowNo { get; set; }
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long CommonHFID { get; set; }
        public string PhaseToearth { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqPhaseToearth { get; set; }
    }
}
