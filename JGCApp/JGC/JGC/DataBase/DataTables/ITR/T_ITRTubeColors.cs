using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRTubeColors")]
    public class T_ITRTubeColors
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long ID { get; set; }
        public long CommonRowID { get; set; }
        public long CommonHFID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string TubeColor { get; set; }
        public string FiberColor { get; set; }
        public int Loss { get; set; }
        public string Remarks { get; set; }
        public long RecordsID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
    }
}
