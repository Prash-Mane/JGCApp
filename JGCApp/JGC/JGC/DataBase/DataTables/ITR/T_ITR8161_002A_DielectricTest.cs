using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8161_002A_DielectricTest")]
    public class T_ITR8161_002A_DielectricTest
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string InsRes1 { get; set; }
        public string AppliedPoint { get; set; }
        public string ChargeCurrent { get; set; }
        public string InsRes2 { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
    }
}
